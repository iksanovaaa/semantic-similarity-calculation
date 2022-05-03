namespace SemanticSimilarityCalculation.Services.Interfaces
{
    public interface ITextProcessingService
    {
        public string GetNormalisedWord(string word);
        public int CountWords(string text);
    }
}
