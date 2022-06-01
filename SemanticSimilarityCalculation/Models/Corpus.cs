using System.Collections.Generic;
using System.Linq;

namespace SemanticSimilarityCalculation.Models
{
    public class Corpus
    {
        public List<Document> Documents { get; set; }

        public Corpus(List<Document> documents)
        {
            this.Documents = documents;
            CreateVectors();
        }

        private void CreateVectors()
        {
            var normalWords = GetAllNormWordsForCorpus();

            foreach (var document in this.Documents)
            {
                document.CreateVector(normalWords);
            }
        }

        private List<List<string>> GetAllNormWordsForCorpus()
        {
            var normalWordsForAnnotationItems = new List<List<string>>();

            if (this.Documents.Where(d => d.Annotations != null).Any())
            {
                var annos = this.Documents
                                .Select(d => d.Annotations
                                                .Select(a => a.Items
                                                                .Select(i => i.NormaTextlWord)
                                                                .Distinct()
                                                                .ToList())
                                                .ToList())
                                .ToList();

                for (int i = 0; i < annos.FirstOrDefault().Count; i++)
                {
                    var annoWords = new List<string>();
                    foreach (var anno in annos)
                    {
                        if (anno[i] != null)
                            annoWords.AddRange(anno[i].Where(aw => aw != null));
                    }
                    normalWordsForAnnotationItems.Add(annoWords.Distinct().ToList());
                }
            }
            
            return normalWordsForAnnotationItems;
        }
    }
}
