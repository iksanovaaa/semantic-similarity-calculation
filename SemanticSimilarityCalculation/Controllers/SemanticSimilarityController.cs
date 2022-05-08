using Microsoft.AspNetCore.Mvc;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services.Interfaces;
using System.Collections.Generic;

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
        [HttpGet]
        public string Ping()
        {
            return "Alive";
        }

        /// <summary>
        /// Рассчитывает семантическую близость между документами корпуса и выводит результат
        /// </summary>
        [HttpPost("CalculateSimilarity")]
        public List<DocumentsSimilarity> CalculateSemanticSimilarity()
        {
            var corpus = _annotationService.GetCorpus();
            var similarityDict = _cosineSimilarityService.GetCorpusSimilarity(corpus);

            return similarityDict;
        }

        /// <summary>
        /// Находит для заданного документа корпуса документы, которые наиболее близки семантически,
        /// и выводит результат
        /// </summary>
        [HttpPost("GetRelevant")]
        public List<DocumentsSimilarity> GetRelevantDocuments(string documentId)
        {
            var corpus = _annotationService.GetCorpus();
            var relevantDocuments = _cosineSimilarityService.GetMostRelevantDocumentsIds(corpus
                                                                                      , documentId);

            return relevantDocuments;
        }
    }
}
