using Newtonsoft.Json.Linq;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticSimilarityCalculation.Services
{
    public class AnnotationService : IAnnotationService
    {
        public List<Document> GetDocumentAnnotationsFromJson(string inputJson)
        {
            List<Document> documents = new List<Document>();
            var items = JArray.Parse(inputJson);
            for (int i = 0; i < items.Count; i++)
            {
                var records = items[i]["annotation"].Children();
                var annotationItems = new List<Annotation>();
                var id = (string)items[i]["id"];
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
            return documents;
        }

        private Annotation GetAnnotation(JToken record)
        {
            var first = record.Children().Children();
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
                foreach (JToken token in first)
                {
                    annotationItem = GetAnnotationItemFromToken(token, term);
                    annotationItems.Items.Add(annotationItem);
                }
            }
            return annotationItems;
        }

        private AnnotationItem GetAnnotationItemFromToken(JToken token, string term)
        {
            AnnotationItem annotationItem = new AnnotationItem();
            if (token.First != null)
            {
                var termEl = token.First;
                var newTerm = (JProperty)termEl;
                var endIndex = (Int64)((JValue)newTerm.Value).Value;
                newTerm = (JProperty)newTerm.Next;
                var startIndex = (Int64)((JValue)newTerm.Value).Value;
                newTerm = (JProperty)newTerm.Next;
                var textWord = (string)((JValue)newTerm.Value).Value;
                newTerm = (JProperty)newTerm.Next;
                var tokenIndex = (Int64)((JValue)newTerm.Value).Value;

                annotationItem = new AnnotationItem((int)startIndex, (int)endIndex
                                                  , textWord, term, (int)tokenIndex);
            }
            else
            {
                annotationItem = new AnnotationItem(term);
            }
            return annotationItem;
        }
    }
}
