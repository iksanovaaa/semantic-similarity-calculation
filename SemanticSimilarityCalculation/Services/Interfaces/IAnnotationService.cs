using SemanticSimilarityCalculation.Models;

namespace SemanticSimilarityCalculation.Services.Interfaces
{
    public interface IAnnotationService
    {
        public Corpus GetCorpusFromAnnotation(string annotation);
    }
}
