using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticSimilarityCalculation.Models
{
    public class DocumentInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DocumentInfo(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
