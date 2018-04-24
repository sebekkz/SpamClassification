using System.Collections.Generic;

namespace ATP2018.SpamClassification.VocabularyGenerators
{
    public interface IVocabularyGenerator
    {
        string[] GetVocabulary(ITokenizer tokenizer, IEnumerable<Sms> texts);
    }
}
