using SemanticSimilarityCalculation.Models;
using System.Collections.Generic;

namespace SemanticSimilarityCalculation.Services.Interfaces
{
    public interface ICosineSimilarityService
    {
        public List<DocumentsSimilarity> GetCorpusSimilarity(Corpus corpus);

        public List<DocumentsSimilarity> GetMostRelevantDocumentsIds(Corpus corpus
                                                                   , string documentId);
    }
}
