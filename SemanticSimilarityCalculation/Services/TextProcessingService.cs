using Pullenti.Morph;
using Pullenti.Ner;
using SemanticSimilarityCalculation.Services.Interfaces;

namespace SemanticSimilarityCalculation.Services
{
    public class TextProcessingService: ITextProcessingService
    {
        private static readonly Processor Processor;
        static TextProcessingService()
        {
            Pullenti.Sdk.InitializeAll();
            Processor = ProcessorService.CreateProcessor();
        }

        public string GetNormalisedWord(string word)
        {
            var normalWord = string.Empty;
            var result = Processor.Process(new SourceOfAnalysis(word));

            var token = result.FirstToken;
            if (token is TextToken)
            {
                if (IsNoun(token))
                {
                    normalWord = token.GetNormalCaseText(MorphClass.Noun, MorphNumber.Singular);
                }
                if (IsAdjective(token))
                {
                    normalWord = token.GetNormalCaseText(MorphClass.Adjective
                                                       , MorphNumber.Singular);
                }
            }

            return normalWord.ToLower();
        }

        public int CountWords(string text)
        {
            var result = Processor.Process(new SourceOfAnalysis(text));
            var token = result.FirstToken;
            var count = 1;
            while (token.Next != null)
            {
                token = token.Next;
                count++;
            }
            return count;
        }

        private bool IsNoun(Token token)
        {
            return token.Morph.Class.IsNoun;
        }

        private bool IsAdjective(Token token)
        {
            return token.Morph.Class.IsAdjective;
        }
    }
}
