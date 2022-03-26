using System.Collections.Generic;

namespace SemanticSimilarityCalculation.Models
{
    public class Annotation
    {
        public List<AnnotationItem> Items { get; set; }

        public Annotation()
        {
            this.Items = new List<AnnotationItem>();
        }

        public Annotation(List<AnnotationItem> items)
        {
            this.Items = items;
        }
    }
}
