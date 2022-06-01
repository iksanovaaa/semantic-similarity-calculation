using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SemanticSimilarityCalculation.Exceptions;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services;
using SemanticSimilarityCalculation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SemanticSimilarityCalculation.Tests.Services
{
    [TestClass]
    public class CosineSimilarityServiceTests
    {
        private ITextProcessingService _textProcessingService = new TextProcessingService();

        private const string DOCUMENT_ID_VAR1 = "ldkjval14r32w";
        private const string DOCUMENT_NAME_VAR1 = "Default name";
        private const string DOCUMENT_ID_VAR2 = "ojkjn82lkmkm";
        private const string DOCUMENT_NAME_VAR2 = "Another default name";
        private readonly List<string> corpusWords = new List<string>() { "мама", "мыла", "раму"
        , "Российская Федерация", "Россия", "Наименования" };

        #region GetCorpusSimilarityTests

        [TestMethod]
        public void GetCorpusSimilarity_DefaultService_TextProcessingServiceNotFoundException()
        {
            // Arrange
            var sut = CreateSut();
            var documents = new List<Document>();
            var document = CreateDocumentVesion1();
            documents.Add(document);
            var corpus = new Corpus(documents);

            // Act and Assert
            Assert.ThrowsException<AnnotationServiceNotFoundException>(() 
                                                                => sut.GetCorpusSimilarity(corpus));
        }

        [TestMethod]
        public void GetCorpusSimilarity_SingleDocument_EmptySimilarityCollection()
        {
            // Arrange
            var annotationService = new AnnotationService(_textProcessingService);
            var sut = CreateSut(annotationService);
            var documents = new List<Document>();
            var document = CreateDocumentVesion1();
            documents.Add(document); 
            var corpus = new Corpus(documents);

            // Act
            var corpusSimilarity = sut.GetCorpusSimilarity(corpus);

            // Assert
            Assert.AreEqual(0, corpusSimilarity.Values.Count);
            Assert.AreEqual(1, corpusSimilarity.Names.Count);
        }

        [TestMethod]
        public void GetCorpusSimilarity_TwoSimilarDocuments_SimilarityIs1()
        {
            // Arrange
            var annotationService = new AnnotationService(_textProcessingService);
            var sut = CreateSut(annotationService);
            var documents = new List<Document>();
            var document = CreateDocumentVesion1();
            documents.Add(document);
            documents.Add(document); 
            var corpus = new Corpus(documents);

            // Act
            var corpusSimilarity = sut.GetCorpusSimilarity(corpus);

            // Assert
            Assert.AreEqual(1, corpusSimilarity.Values.Count);
            Assert.AreEqual(1, Math.Round(corpusSimilarity.Values.Single().Similarity, 1));
            Assert.AreEqual(2, corpusSimilarity.Names.Count);
        }

        [TestMethod]
        public void GetCorpusSimilarity_TwoDifferentDocuments_SimilarityIs0()
        {
            // Arrange
            var annotationService = new AnnotationService(_textProcessingService);
            var sut = CreateSut(annotationService);
            var documents = new List<Document>();
            var document1 = CreateDocumentVesion1();
            var document2 = CreateDocumentVesion2();
            documents.Add(document1);
            documents.Add(document2);
            var corpus = new Corpus(documents);

            // Act
            var corpusSimilarity = sut.GetCorpusSimilarity(corpus);
            var a = 1.0 * (int)0.9 + 10.0f;
            // Assert
            Assert.AreEqual(1, corpusSimilarity.Values.Count);
            Assert.AreEqual(0, corpusSimilarity.Values.Single().Similarity);
            Assert.AreEqual(2, corpusSimilarity.Names.Count);
        }

        #endregion GetCorpusSimilarityTests

        #region GetMostRelevantDocumentsIds

        [TestMethod]
        public void GetMostRelevantDocumentsIds_EmptyCorpus_EmptyCollection()
        {
            // Arrange
            var sut = CreateSut();
            var documents = new List<Document>();
            var corpus = new Corpus(documents);

            // Act
            var relevantDocuments = sut.GetMostRelevantDocumentsIds(corpus, DOCUMENT_ID_VAR1);

            // Assert
            Assert.AreEqual(0, relevantDocuments.Count);
        }

        [TestMethod]
        public void GetMostRelevantDocumentsIds_SingleDocument_EmptyCollection()
        {
            // Arrange
            var sut = CreateSut();
            var documents = new List<Document>();
            var document = CreateDocumentVesion1();
            documents.Add(document);
            var corpus = new Corpus(documents);

            // Act
            var relevantDocuments = sut.GetMostRelevantDocumentsIds(corpus, DOCUMENT_ID_VAR1);

            // Assert
            Assert.AreEqual(0, relevantDocuments.Count);
        }

        [TestMethod]
        public void GetMostRelevantDocumentsIds_SimilarDocuments_SimilarDocumentsCollection()
        {
            // Arrange
            var sut = CreateSut();
            var documents = new List<Document>();
            var document = CreateDocumentVesion1();
            documents.Add(document);
            documents.Add(document);
            documents.Add(document);
            var corpus = new Corpus(documents);

            // Act
            var relevantDocuments = sut.GetMostRelevantDocumentsIds(corpus, DOCUMENT_ID_VAR1);

            // Assert
            Assert.AreEqual(3, relevantDocuments.Count);
        }

        [TestMethod]
        public void GetMostRelevantDocumentsIds_NotSimilarDocuments_EmptyCollection()
        {
            // Arrange
            var sut = CreateSut();
            var documents = new List<Document>();
            var document1 = CreateDocumentVesion1();
            var document2 = CreateDocumentVesion2();
            documents.Add(document1);
            documents.Add(document2);
            var corpus = new Corpus(documents);

            // Act
            var relevantDocuments = sut.GetMostRelevantDocumentsIds(corpus, DOCUMENT_ID_VAR1);

            // Assert
            Assert.AreEqual(0, relevantDocuments.Count);
        }

        [DataTestMethod]
        [DataRow(DOCUMENT_ID_VAR1, 1)]
        [DataRow(DOCUMENT_ID_VAR2, 0)]
        public void GetMostRelevantDocumentsIds_DifferentDocuments_SimilarDocumentsCollection(
                                                    string documentId, int relevantDocumentsCount)
        {
            // Arrange
            var sut = CreateSut();
            var documents = new List<Document>();
            var document1 = CreateDocumentVesion1();
            var document2 = CreateDocumentVesion2();
            documents.Add(document1);
            documents.Add(document1);
            documents.Add(document2);
            var corpus = new Corpus(documents);

            // Act
            var relevantDocuments = sut.GetMostRelevantDocumentsIds(corpus, documentId);

            // Assert
            Assert.AreEqual(relevantDocumentsCount, relevantDocuments.Count);
        }

        #endregion GetMostRelevantDocumentsIds

        #region Helpers

        private ISimilarityService CreateSut(IAnnotationService annotationService = null)
        {
            return new CosineSimilarityService(annotationService);
        }

        private Document CreateDocumentVesion1()
        {
            var words = new List<string>() { "мама", "мыла", "раму" };
            var annotation = GetAnnotation(words, "мама");
            var annotations = new List<Annotation>() { annotation };
            var document = GetDocument(DOCUMENT_ID_VAR1, DOCUMENT_NAME_VAR1, annotations
                                                                           , "мама мыла раму");
            var wordsList = new List<List<string>>() { corpusWords };
            document.CreateVector(wordsList);

            return document;
        }

        private Document CreateDocumentVesion2()
        {
            var words1 = new List<string>() { "Российская Федерация", "Россия" };
            var annotation1 = GetAnnotation(words1, "страна");
            var words2 = new List<string>() { "Наименования" };
            var annotation2 = GetAnnotation(words2, "имя");
            var annotations = new List<Annotation>() { annotation1, annotation2 };
            var document = GetDocument(DOCUMENT_ID_VAR2, DOCUMENT_NAME_VAR2, annotations
                                        , "Наименования Российская Федерация и Россия равнозначны");
            var wordsList = new List<List<string>>() { corpusWords };
            document.CreateVector(wordsList);

            return document;
        }

        private static Document GetDocument(string id, string name, List<Annotation> annotations
                                                                                   , string text) =>
            Builder<Document>.CreateNew()
                .And(d => d.Id = id)
                .And(d => d.Name = name)
                .And(d => d.Annotations = annotations)
                .And(d => d.Text = text)
                .Build();

        private Annotation GetAnnotation(List<string> words, string ontologyTerm)
        {
            var annotation = new Annotation();

            foreach (var word in words)
            {
                var annotationItem = GetAnnotationItem(word, ontologyTerm);
                annotation.Items.Add(annotationItem);
            }

            return annotation;
        }

        private AnnotationItem GetAnnotationItem(string word, string ontologyTerm)
        {
            var normalWord = _textProcessingService.GetNormalisedWord(word);
            return new AnnotationItem(1, 1, word, normalWord, ontologyTerm, 1);
        }

        #endregion Helpers
    }
}
