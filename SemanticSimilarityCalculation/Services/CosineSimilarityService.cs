using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SemanticSimilarityCalculation.Services
{
    public class CosineSimilarityService : ICosineSimilarityService
    {
        private const double MIN_SIMILARITY_VALUE = 0.35;

        public List<DocumentsSimilarity> GetCorpusSimilarity(Corpus corpus)
        {
            var documentsSimilarities = new List<DocumentsSimilarity>();
            var vectors = GetVectorsFromCorpus(corpus);

            for (int i = 0; i < corpus.Documents.Count - 1; i++)
            {
                for (int j = i + 1; j < corpus.Documents.Count; j++)
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
            var similarityList = GetCorpusSimilarity(corpus);
            var relevantDocuments = similarityList.Where(ds => (ds.FirstDocumentId == documentId
                                                            || ds.SecondDocumentId == documentId)
                                                            && ds.Similarity > MIN_SIMILARITY_VALUE)
                                                   .OrderByDescending(ds => ds.Similarity)
                                                   .ToList();

            for (int i = 0; i < relevantDocuments.Count; i++)
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
            var maxValue = vectors.Max(vector => vector.Max());
            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = NormalizeVector(vectors[i], maxValue);
            }
            return vectors;
        }

        private List<double> NormalizeVector(List<double> vector, double maxValue)
        {
            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] /= maxValue;
            }
            return vector;
        }

        private double GetVectorsScalar(List<double> vector1, List<double> vector2)
        {
            double scalar = 0;
            for (int i = 0; i < vector1.Count; i++)
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
