namespace ATP2018.SpamClassification
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ToLowerWordsTokenizer : ITokenizer
    {
        private Regex regex = new Regex(@"\w+");

        public HashSet<string> Tokenize(string text)
        {
            var matches = this.regex.Matches(text.ToLowerInvariant());
            var tokens
                = matches
                .Cast<Match>()
                .Select(x => x.Value);

            return new HashSet<string>(tokens);
        }
    }
}
