using System;

namespace SemanticSimilarityCalculation.Exceptions
{
    public class TextProcessingServiceNotFoundException : ApplicationException
    {
        public TextProcessingServiceNotFoundException(string message) 
            : base(message)
        {

        }
    }
}
