namespace ATP2018.SpamClassification
{
    using System.Collections.Generic;

    public interface ITokenizer
    {
        HashSet<string> Tokenize(string text);
    }
}