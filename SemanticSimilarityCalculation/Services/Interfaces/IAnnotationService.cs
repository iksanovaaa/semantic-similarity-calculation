using SemanticSimilarityCalculation.Models;
using System.Collections.Generic;

namespace SemanticSimilarityCalculation.Services.Interfaces
{
    public interface IAnnotationService
    {
        public Corpus GetCorpus();
        public List<DocumentInfo> GetDocumentsNames(Corpus corpus);
    }
}
