using Newtonsoft.Json.Linq;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace SemanticSimilarityCalculation.Services
{
    public class AnnotationService : IAnnotationService
    {
        private readonly ITextProcessingService _textProcessingService;

        public AnnotationService(ITextProcessingService textProcessingService)
        {
            _textProcessingService = textProcessingService;
        }

        public Corpus GetCorpusFromAnnotation(string annotation)
        {
            var json = GetJson(annotation);
            var corpus = GetCorpusFromJson(json);

            return corpus;
        }

        private string GetJson(string annotation)
        {
            var json = string.Empty;
            using (StreamReader r = new StreamReader(annotation))
            {
                json = r.ReadToEnd();
            }

            return json;
        }

        private Corpus GetCorpusFromJson(string inputJson)
        {
            List<Document> documents = new List<Document>();
            var items = JArray.Parse(inputJson);
            for (int i = 0; i < items.Count; i++)
            {
                var records = items[i]["annotation"].Children();
                var annotationItems = new List<Annotation>();
                var id = (string)items[i]["_id"]["$oid"];
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
            AnnotationItem annotationItem = new AnnotationItem();
            var annotationItems = new Annotation();
            var path = record.Path.Split('.');
            var term = path[path.Length - 1];

            if (!record.First.HasValues)
            {
                annotationItem.OntologyTerm = term;
                annotationItems.Items.Add(annotationItem);
            }
            else
            {
                foreach (JToken token in annotationToken)
                {
                    annotationItem = GetAnnotationItemFromToken(token, term);
                    annotationItems.Items.Add(annotationItem);
                }
            }
            return annotationItems;
        }

        private AnnotationItem GetAnnotationItemFromToken(JToken token, string term)
        {
            var annotationItem = new AnnotationItem();
            if (token.First != null)
            {
                var termElement = token.First;
                var newTerm = (JProperty)termElement;
                var textWord = (string)((JValue)newTerm.Value).Value;
                var normalTextWord = _textProcessingService.GetNormalisedWord(textWord);
                newTerm = (JProperty)newTerm.Next;
                var startIndex = (long)((JValue)newTerm.Value).Value;
                newTerm = (JProperty)newTerm.Next;
                var endIndex = (long)((JValue)newTerm.Value).Value;
                newTerm = (JProperty)newTerm.Next;
                var tokenIndex = (long)((JValue)newTerm.Value).Value;

                annotationItem = new AnnotationItem((int)startIndex, (int)endIndex
                                                 , textWord, normalTextWord, term, (int)tokenIndex);
            }
            else
            {
                annotationItem = new AnnotationItem(term);
            }
            return annotationItem;
        }
    }
}
