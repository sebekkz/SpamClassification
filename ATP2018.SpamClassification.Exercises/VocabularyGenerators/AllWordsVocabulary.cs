namespace ATP2018.SpamClassification.VocabularyGenerators
{
    using System.Collections.Generic;
    using System.Linq;

    public class AllWordsVocabulary : IVocabularyGenerator
    {
        public string[] GetVocabulary(ITokenizer tokenizer, IEnumerable<Sms> smses)
        {
            return smses
                .SelectMany(x => tokenizer.Tokenize(x.Text))
                .Distinct()
                .ToArray();
        }
    }
}
