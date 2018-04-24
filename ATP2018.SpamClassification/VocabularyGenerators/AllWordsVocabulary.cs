namespace ATP2018.SpamClassification.VocabularyGenerators
{
    using System.Collections.Generic;
    using System.Linq;

    public class AllWordsVocabulary : IVocabularyGenerator
    {
        public string[] GetVocabulary(ITokenizer tokenizer, IEnumerable<string> texts)
        {
            return texts
                .SelectMany(x => tokenizer.Tokenize(x))
                .Distinct()
                .ToArray();
        }
    }
}
