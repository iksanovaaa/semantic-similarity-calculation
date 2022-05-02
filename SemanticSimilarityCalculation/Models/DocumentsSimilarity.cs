namespace SemanticSimilarityCalculation.Models
{
    public class DocumentsSimilarity
    {
        public string FirstDocumentId { get; set; }
        public string SecondDocumentId { get; set; }
        public double Similarity { get; set; }

        public DocumentsSimilarity(string firstDocumentId, string secondDocumentId
                                                               , double similarity)
        {
            this.FirstDocumentId = firstDocumentId;
            this.SecondDocumentId = secondDocumentId;
            this.Similarity = similarity;
        }
    }
}
