using Microsoft.AspNetCore.Mvc;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace SemanticSimilarityCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemanticSimilarityController : Controller
    {
        private readonly IAnnotationService _annotationService;
        private readonly ICosineSimilarityService _cosineSimilarityService;

        public SemanticSimilarityController(IAnnotationService annotationService
                                          , ICosineSimilarityService cosineSimilarityService)
        {
            _annotationService = annotationService;
            _cosineSimilarityService = cosineSimilarityService;
        }

        /// <summary>
        /// Проверяет работоспособность контролера
        /// </summary>
        public string Ping()
        {
            return "Alive";
        }

        /// <summary>
        /// Рассчитывает семантическую близость между документами корпуса и выводит результат
        /// </summary>
        [HttpPost("Calculate")]
        public List<DocumentsSimilarity> CalculateSemanticSimilarity([FromQuery] string annotation)
        {
            var json = string.Empty;
            using (StreamReader r = new StreamReader(annotation))
            {
                json = r.ReadToEnd();
            }
            var documents = _annotationService.GetDocumentAnnotationsFromJson(json);
            var similarityDict = _cosineSimilarityService.GetCorpusSimilarity(documents);

            return similarityDict;
        }

        [HttpPost("GetRelevant")]
        public List<DocumentsSimilarity> CalculateSemanticSimilarity([FromQuery] string annotation
                                                                  , string documentId)
        {
            var json = string.Empty;
            using (StreamReader r = new StreamReader(annotation))
            {
                json = r.ReadToEnd();
            }
            var documents = _annotationService.GetDocumentAnnotationsFromJson(json);
            var relevantDocuments = _cosineSimilarityService.GetMostRelevantDocumentsIds(documents
                                                                                      , documentId);

            return relevantDocuments;
        }
    }
}
