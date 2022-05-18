using SemanticSimilarityCalculation.Models;
using System.Collections.Generic;

namespace SemanticSimilarityCalculation.Services.Interfaces
{
    public interface ISimilarityService
    {
        public CorpusSimilarity GetCorpusSimilarity(Corpus corpus);
        public List<DocumentsSimilarity> GetDocumentsSimilarities(Corpus corpus);
        public List<DocumentsSimilarity> GetMostRelevantDocumentsIds(Corpus corpus
                                                                   , string documentId);
    }
}
