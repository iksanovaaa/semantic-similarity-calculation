using Newtonsoft.Json;

namespace SemanticSimilarityCalculation.Models
{
    public class AnnotationItem
    {
        public int? StartIndex { get; set; }
        public int? EndIndex { get; set; }
        public string TextWord { get; set; }
        public string OntologyTerm { get; set; }
        public int? TokenIndex { get; set; }

        public AnnotationItem(int startIndex, int endIndex, string textWord, string ontologyTerm
                                                                                , int tokenIndex)
        {
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
            this.TextWord = textWord;
            this.OntologyTerm = ontologyTerm;
            this.TokenIndex = tokenIndex;
        }

        public AnnotationItem(string ontologyTerm)
        {
            this.StartIndex = null;
            this.EndIndex = null;
            this.TextWord = string.Empty;
            this.OntologyTerm = ontologyTerm;
            this.TokenIndex = null;
        }

        public AnnotationItem() { }

        public AnnotationItem GetAnnotationItemFromJson(string jsonInput)
        {
            var annotationItem = JsonConvert.DeserializeObject<AnnotationItem>(jsonInput);
            return annotationItem;
        }

        public override string ToString()
        {
            var startIndexTxt = $"start_index: {this.StartIndex}\n";
            var endIndexTxt = $"end_index: {this.EndIndex}\n";
            var textWordTxt = $"text_word: {this.TextWord}\n";
            var ontoogyTermTxt = $"token_word: {this.OntologyTerm}\n";
            var tokenIndexTxt = $"token_index: {this.TokenIndex}";

            return $"{startIndexTxt}{endIndexTxt}{textWordTxt}{ontoogyTermTxt}{tokenIndexTxt}";
        }
    }
}
