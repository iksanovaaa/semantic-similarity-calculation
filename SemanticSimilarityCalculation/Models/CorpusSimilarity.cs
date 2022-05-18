using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticSimilarityCalculation.Models
{
    public class CorpusSimilarity
    {
        public List<DocumentsSimilarity> Values { get; set; }
        public List<DocumentInfo> Names { get; set; }

        public CorpusSimilarity(List<DocumentsSimilarity> values, List<DocumentInfo> names)
        {
            this.Values = values;
            this.Names = names;
        }
    }
}
