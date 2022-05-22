using Microsoft.VisualStudio.TestTools.UnitTesting;
using SemanticSimilarityCalculation.Services;
using SemanticSimilarityCalculation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SemanticSimilarityCalculation.Tests.Services
{
    [TestClass]
    public class TextProcessingServiceTests
    {
        [DataTestMethod]
        [DataRow("Юридического", "юридический")]
        [DataRow("", "")]
        [DataRow("кодЕксом", "кодекс")]
        [DataRow("абвгде", "")]
        [DataRow("Developments", "development")]
        [DataRow("42", "")]
        public void GetNormalisedWord_Word_NormalWord(string word, string expectedNormalWord)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetNormalisedWord(word);

            // Assert
            Assert.AreEqual(expectedNormalWord, result);
        }

        [DataTestMethod]
        [DataRow("мама мыла раму", 3)]
        [DataRow("Российская Федерация - Россия есть демократическое федеративное правовое государство с республиканской формой правления.", 11)]
        [DataRow("", 0)]
        [DataRow("а о ы е у", 5)]
        [DataRow("London is the capital of Great Britain", 6)]
        public void CountWords_Text_WordCount(string text, int expectedWordCount)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.CountWords(text);

            // Assert
            Assert.AreEqual(expectedWordCount, result);
        }

        private ITextProcessingService CreateSut()
        {
            return new TextProcessingService();
        }
    }
}
