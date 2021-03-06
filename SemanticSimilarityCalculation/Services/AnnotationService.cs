using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using SemanticSimilarityCalculation.Exceptions;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticSimilarityCalculation.Services
{
    public class AnnotationService : IAnnotationService
    {
        private readonly ITextProcessingService _textProcessingService;

        public AnnotationService(ITextProcessingService textProcessingService)
        {
            _textProcessingService = textProcessingService;
        }

        public Corpus GetCorpus()
        {
            var json = GetJson();
            var corpus = GetCorpusFromJson(json);

            return corpus;
        }

        public List<DocumentInfo> GetDocumentsNames(Corpus corpus)
        {
            var documentsNames = corpus.Documents.Select(d => new DocumentInfo(d.Id, d.Name))
                                                 .ToList();

            return documentsNames;
        }

        #region Get JSON from database

        private string GetJson()
        {
            var connectionString = GetConnectionString();
            var annotations = GetAnnotationsFromDataBase(connectionString).Result;
            var json = $"[{annotations}]";

            return json.ToString();
        }

        private string GetConnectionString()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory
                                       .Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }

        private async Task<string> GetAnnotationsFromDataBase(string connectionString)
        {
            var annotations = new StringBuilder();

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("user_shopping_list");
            var collection = db.GetCollection<BsonDocument>("annotations");
            var filter = new BsonDocument();

            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var count = 1;
                    foreach (var document in cursor.Current)
                    {
                        var clearDocument = document.ToString().Replace("ObjectId(", string.Empty)
                                                               .Replace("), \"id\"", ", \"id\"")
                                                               .Replace("}}", "}");
                        annotations.Append(clearDocument);
                        if (count < cursor.Current.Count())
                        {
                            annotations.Append(",");
                            count++;
                        }
                    }
                }
            }

            return annotations.ToString();
        }

        #endregion Get JSON from database

        private Corpus GetCorpusFromJson(string inputJson)
        {
            List<Document> documents = new List<Document>();
            var items = JArray.Parse(inputJson);
            for (int i = 0; i < items.Count; i++)
            {
                var records = items[i]["annotation"].Children();
                var annotationItems = new List<Annotation>();
                var id = (string)items[i]["_id"];
                var name = (string)items[i]["name"];
                var text = (string)items[i]["text"];

                foreach (JToken record in records)
                {
                    var annotationList = GetAnnotation(record);
                    annotationItems.Add(annotationList);
                }

                var document = new Document(id, annotationItems, name, text);
                documents.Add(document);
            }
            return new Corpus(documents);
        }

        private Annotation GetAnnotation(JToken record)
        {
            var annotationToken = record.Children().Children();
            var annotationItems = new Annotation();
            var path = record.Path.Split('.');
            var term = path[path.Length - 1];

            if (record.First.HasValues)
            {
                foreach (JToken token in annotationToken)
                {
                    var annotationItem = GetAnnotationItemFromToken(token, term);
                    annotationItems.Items.Add(annotationItem);
                }
            }
            else
            {
                var annotationItem = new AnnotationItem(term);
                annotationItems.Items.Add(annotationItem);
            }

            return annotationItems;
        }

        private AnnotationItem GetAnnotationItemFromToken(JToken token, string term)
        {
            var annotationItem = new AnnotationItem(term);

            if (token.First != null)
            {
                var termElement = token.First;
                var newTerm = (JProperty)termElement;
                var textWord = (string)((JValue)newTerm.Value).Value.ToString().ToLower();
                var normalTextWord = GetNormalWordForAnnotationItem(textWord);
                newTerm = (JProperty)newTerm.Next;
                var startIndex = GetNumberFromTerm(ref newTerm);
                var endIndex = GetNumberFromTerm(ref newTerm);
                var tokenIndex = GetNumberFromTerm(ref newTerm);

                annotationItem = new AnnotationItem(startIndex, endIndex, textWord, normalTextWord
                                                  , term, tokenIndex);
            }

            return annotationItem;
        }

        private string GetNormalWordForAnnotationItem(string textWord)
        {
            var normalTextWord = string.Empty;

            try
            {
                normalTextWord = _textProcessingService.GetNormalisedWord(textWord);
            }
            catch
            {
                var exceptionMessage = "Сервис для обработки текста не найден.";
                throw new TextProcessingServiceNotFoundException(exceptionMessage);
            }

            return normalTextWord;
        }

        private int GetNumberFromTerm(ref JProperty term)
        {
            var number = (long)((JValue)term.Value).Value;
            term = (JProperty)term.Next;

            return (int)number;
        }
    }
}
