namespace SemanticSimilarityCalculation.Models
{
    public class AnnotationItem
    {
        public int? StartIndex { get; set; }
        public int? EndIndex { get; set; }
        public string TextWord { get; set; }
        public string OntologyTerm { get; set; }
        public int? TokenIndex { get; set; }
        public string NormaTextlWord { get; }

        public AnnotationItem(int startIndex, int endIndex, string textWord, string normalTextWord
                                                            , string ontologyTerm, int tokenIndex)
        {
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
            this.TextWord = textWord;
            this.NormaTextlWord = normalTextWord;
            this.OntologyTerm = ontologyTerm;
            this.TokenIndex = tokenIndex;
        }

        public AnnotationItem(string ontologyTerm)
        {
            this.StartIndex = null;
            this.EndIndex = null;
            this.TextWord = null;
            this.NormaTextlWord = null;
            this.OntologyTerm = ontologyTerm;
            this.TokenIndex = null;
        }
    }
}
