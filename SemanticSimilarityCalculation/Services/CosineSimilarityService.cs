using SemanticSimilarityCalculation.Exceptions;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SemanticSimilarityCalculation.Services
{
    public class CosineSimilarityService : ISimilarityService
    {
        private const double MIN_SIMILARITY_VALUE = 0.35;

        public IAnnotationService _annotationService;

        public CosineSimilarityService(IAnnotationService annotationService)
        {
            _annotationService = annotationService;
        }

        public CorpusSimilarity GetCorpusSimilarity(Corpus corpus)
        {
            var corpusSimilarityValues = GetDocumentsSimilarities(corpus);

            var corpusNames = GetCorpusNames(corpus);

            var corpusSimilarity = new CorpusSimilarity(corpusSimilarityValues, corpusNames);

            return corpusSimilarity;
        }

        public List<DocumentsSimilarity> GetDocumentsSimilarities(Corpus corpus)
        {
            var documentsSimilarities = new List<DocumentsSimilarity>();
            var vectors = GetVectorsFromCorpus(corpus);

            for (var i = 0; i < corpus.Documents.Count - 1; i++)
            {
                for (var j = i + 1; j < corpus.Documents.Count; j++)
                {
                    var cosineSimilarity = GetCosineSimilarity(vectors[i], vectors[j]);
                    var documentsSimilarity = new DocumentsSimilarity(corpus.Documents[i].Id
                                                        , corpus.Documents[j].Id, cosineSimilarity);
                    documentsSimilarities.Add(documentsSimilarity);
                }
            }

            return documentsSimilarities;
        }

        public List<DocumentsSimilarity> GetMostRelevantDocumentsIds(Corpus corpus
                                                                       , string documentId)
        {
            var similarityList = GetDocumentsSimilarities(corpus);
            var relevantDocuments = similarityList.Where(ds => (ds.FirstDocumentId == documentId
                                                            || ds.SecondDocumentId == documentId)
                                                            && ds.Similarity > MIN_SIMILARITY_VALUE)
                                                   .OrderByDescending(ds => ds.Similarity)
                                                   .ToList();

            for (var i = 0; i < relevantDocuments.Count; i++)
            {
                if (relevantDocuments[i].SecondDocumentId == documentId)
                {
                    var relevantDocumentId = relevantDocuments[i].FirstDocumentId;
                    relevantDocuments[i].FirstDocumentId = relevantDocuments[i].SecondDocumentId;
                    relevantDocuments[i].SecondDocumentId = relevantDocumentId;
                }
            }

            return relevantDocuments;
        }

        private List<DocumentInfo> GetCorpusNames(Corpus corpus)
        {
            try
            {
                var corpusNames = _annotationService.GetDocumentsNames(corpus);
                return corpusNames;
            }
            catch
            {
                var exceptionMessage = "Сервис для работы с аннотациями не найден.";
                throw new AnnotationServiceNotFoundException(exceptionMessage);
            }
        }

        private List<List<double>> GetVectorsFromCorpus(Corpus corpus)
        {
            var vectors = new List<List<double>>();

            foreach (var document in corpus.Documents)
            {
                vectors.Add(document.Vector);
            }

            vectors = NormalizeVectors(vectors);
            return vectors;
        }

        private List<List<double>> NormalizeVectors(List<List<double>> vectors)
        {
            if (vectors.Any())
            {
                var maxValue = vectors.Max(vector => vector.Max());
                for (var i = 0; i < vectors.Count; i++)
                {
                    vectors[i] = NormalizeVector(vectors[i], maxValue);
                }
            }
            return vectors;
        }

        private List<double> NormalizeVector(List<double> vector, double maxValue)
        {
            for (var i = 0; i < vector.Count; i++)
            {
                vector[i] /= maxValue;
            }
            return vector;
        }

        private double GetVectorsScalar(List<double> vector1, List<double> vector2)
        {
            double scalar = 0;
            for (var i = 0; i < vector1.Count; i++)
            {
                scalar += (vector1[i] * vector2[i]);
            }
            return scalar;
        }

        private double GetVectorModule(List<double> vector)
        {
            double module = 0;
            foreach (var element in vector)
            {
                module += Math.Pow(element, 2);
            }
            module = Math.Sqrt(module);
            return module;
        }

        private double GetCosineSimilarity(List<double> vector1, List<double> vector2)
        {
            var vector1Module = GetVectorModule(vector1);
            var vector2Module = GetVectorModule(vector2);
            var scalar = GetVectorsScalar(vector1, vector2);
            var cosine = scalar / (vector1Module * vector2Module);
            return cosine;
        }
    }
}
