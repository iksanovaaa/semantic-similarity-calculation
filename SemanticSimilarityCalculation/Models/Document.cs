using SemanticSimilarityCalculation.Services;
using SemanticSimilarityCalculation.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SemanticSimilarityCalculation.Models
{
    public class Document
    {
        private static readonly ITextProcessingService _textProcessingService 
                                                                = new TextProcessingService();

        public string Id { get; set; }
        public List<Annotation> Annotations { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<double> Vector { get; set; }
        public int WordCount { get; set; }

        public Document(string id, List<Annotation> annotations, string name, string text)
        {
            this.Id = id;
            this.Annotations = annotations;
            this.Name = name;
            this.Text = text;
            this.WordCount = _textProcessingService.CountWords(this.Text);
        }

        public void CreateVector(IEnumerable<IEnumerable<string>> words)
        {
            var wordsArr = words.ToArray();
            var vector = new List<double>();

            for (int i = 0; i < wordsArr.Count(); i++)
            {
                var annoWords = wordsArr[i];
                foreach (var word in annoWords)
                {
                    var wordsToCount = this.Annotations.ToArray()[i].Items
                               .Where(i => i.NormaTextlWord == word)
                               .Select(i => i.NormaTextlWord)
                               .ToList();
                    var number = (wordsToCount.Count() / (double)this.WordCount * 1000);
                    vector.Add(number);
                }
            }

            this.Vector = vector;
        }
    }
}
