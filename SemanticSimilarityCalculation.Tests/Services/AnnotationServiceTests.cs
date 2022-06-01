using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SemanticSimilarityCalculation.Exceptions;
using SemanticSimilarityCalculation.Models;
using SemanticSimilarityCalculation.Services;
using SemanticSimilarityCalculation.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SemanticSimilarityCalculation.Tests
{
    [TestClass]
    public class AnnotationServiceTests
    {
        private const string DOCUMENT_ID_VAR1 = "ldkjval14r32w";
        private const string DOCUMENT_NAME_VAR1 = "Default name";
        private const string DOCUMENT_ID_VAR2 = "a;dskfe;we1";
        private const string DOCUMENT_NAME_VAR2 = "Another name";
        private const string DOCUMENT_ID_VAR3 = "kjnmnef097657";
        private const string DOCUMENT_NAME_VAR3 = "Yet another name";

        [TestMethod]
        public void GetCorpus_DefaultService_TextProcessingServiceNotFoundException()
        {
            // Arrange
            var sut = CreateSut();

            // Act and Assert
            Assert.ThrowsException<TextProcessingServiceNotFoundException>(() => sut.GetCorpus());
        }

        [TestMethod]
        public void GetCorpus_TextProcessingService_NotNullCorpus()
        {
            // Arrange
            var stubTextProcessingService = new Mock<ITextProcessingService>();
            var sut = CreateSut(stubTextProcessingService.Object);

            // Act
            var result = sut.GetCorpus();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCorpus_TextProcessingService_NotNullCorpus1()
        {
            // Arrange
            var stubTextProcessingService = new Mock<ITextProcessingService>();
            var sut = CreateSut(stubTextProcessingService.Object);

            // Act
            var result = sut.GetCorpus();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDocumentsNames_TextProcessingService_DocumentInfos()
        {
            // Arrange
            var stubTextProcessingService = new Mock<ITextProcessingService>();
            var sut = CreateSut(stubTextProcessingService.Object);
            var documents = GetDocuments();
            var corpus = new Corpus(documents);
            var expectedResult = corpus.Documents.Select(d => new DocumentInfo(d.Id, d.Name))
                                                 .ToList();

            // Act
            var result = sut.GetDocumentsNames(corpus);

            // Assert
            Assert.AreEqual(expectedResult.Count, result.Count);
            for (var i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[0].Id, result[0].Id);
                Assert.AreEqual(expectedResult[0].Name, result[0].Name);
            }
        }

        private static Document GetDocument(string id, string name) =>
            Builder<Document>.CreateNew()
                .And(d => d.Id = id)
                .And(d => d.Name = name)
                .Build();

        private IAnnotationService CreateSut(ITextProcessingService textProcessingService = null)
        {
            return new AnnotationService(textProcessingService);
        }

        private List<Document> GetDocuments()
        {
            var documents = new List<Document>();

            var document11 = GetDocument(DOCUMENT_ID_VAR1, DOCUMENT_NAME_VAR1);
            var document21 = GetDocument(DOCUMENT_ID_VAR2, DOCUMENT_NAME_VAR1);
            var document31 = GetDocument(DOCUMENT_ID_VAR3, DOCUMENT_NAME_VAR1);

            var document12 = GetDocument(DOCUMENT_ID_VAR1, DOCUMENT_NAME_VAR2);
            var document22 = GetDocument(DOCUMENT_ID_VAR2, DOCUMENT_NAME_VAR2);
            var document32 = GetDocument(DOCUMENT_ID_VAR3, DOCUMENT_NAME_VAR2);

            var document13 = GetDocument(DOCUMENT_ID_VAR1, DOCUMENT_NAME_VAR3);
            var document23 = GetDocument(DOCUMENT_ID_VAR2, DOCUMENT_NAME_VAR3);
            var document33 = GetDocument(DOCUMENT_ID_VAR3, DOCUMENT_NAME_VAR3);

            documents.Add(document11);
            documents.Add(document21);
            documents.Add(document31);

            documents.Add(document12);
            documents.Add(document22);
            documents.Add(document32);

            documents.Add(document13);
            documents.Add(document23);
            documents.Add(document33);

            return documents;
        }
    }
}
