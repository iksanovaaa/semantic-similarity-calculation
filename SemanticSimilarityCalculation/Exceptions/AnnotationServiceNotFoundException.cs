using System;

namespace SemanticSimilarityCalculation.Exceptions
{
    public class AnnotationServiceNotFoundException :ApplicationException
    {
        public AnnotationServiceNotFoundException(string message)
            : base(message)
        {

        }
    }
}
