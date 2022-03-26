using System.Collections.Generic;
using System.Linq;

namespace SemanticSimilarityCalculation.Models
{
    public class Document
    {
        public string Id { get; set; }
        public List<Annotation> Annotation { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<double> Vector
        {
            get
            {
                var vector = new List<double>();
                foreach (var annotationItem in this.Annotation)
                {
                    var itemWords = annotationItem.Items
                                        .Where(item => item.TextWord != null)
                                        .Count();
                    if (itemWords != 0)
                    {
                        vector.Add(itemWords);
                    }
                    else
                    {
                        vector.Add(0);
                    }
                }

                return vector;
            }
        }

        public Document(string id, List<Annotation> annotation, string name, string text)
        {
            this.Id = id;
            this.Annotation = annotation;
            this.Name = name;
            this.Text = text;
        }

        public override string ToString()
        {
            var id = $"id: {this.Id}\n";
            var name = $"name: {this.Name}\n";
            var text = $"text: {this.Text}\n";

            return $"{id}{name}{text}";
        }
    }
}
