using System.Collections.Generic;

namespace ATP2018.SpamClassification
{
    public interface ITokenizer
    {
        HashSet<string> Tokenize(string text);
    }
}