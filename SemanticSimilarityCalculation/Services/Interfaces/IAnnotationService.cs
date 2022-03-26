using SemanticSimilarityCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticSimilarityCalculation.Services.Interfaces
{
    public interface IAnnotationService
    {
        public List<Document> GetDocumentAnnotationsFromJson(string inputJson);
    }
}
