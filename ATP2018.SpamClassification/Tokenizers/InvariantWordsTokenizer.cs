using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ATP2018.SpamClassification
{
    public class InvariantWordsTokenizer : ITokenizer
    {
        private Regex regex = new Regex(@"\w+");

        public HashSet<string> Tokenize(string text)
        {
            var matches = regex.Matches(text.ToLowerInvariant());
            var tokens 
                = matches
                .Cast<Match>()
                .Select(x => x.Value);

            return new HashSet<string>(tokens);
        }
    }
}
