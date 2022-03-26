using SemanticSimilarityCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticSimilarityCalculation.Services.Interfaces
{
    public interface ICosineSimilarityService
    {
        public List<DocumentsSimilarity> GetCorpusSimilarity(List<Document> corpus);

        public List<DocumentsSimilarity> GetMostRelevantDocumentsIds(List<Document> corpus
                                                                       , string documentId);
    }
}
