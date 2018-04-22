using System;
using System.Collections.Generic;
using System.Linq;

namespace ATP2018.SpamClassification.Classifiers
{
    public class NaiveBayes : IClassifier
    {
        private readonly ITokenizer tokenizer;
        private readonly Dictionary<SmsLabel, Group> groups;

        public NaiveBayes(ITokenizer tokenizer, Dictionary<SmsLabel, Group> groups)
        {
            this.tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
            this.groups = groups;
        }

        public SmsLabel Classify(string text)
        {
            var tokenizedSms = tokenizer.Tokenize(text);

            return groups
                .OrderByDescending(x => Score(tokenizedSms, x.Value))
                .First()
                .Key;
        }

        private double TokenScore(Group group, string token)
        {
            group
                .TokenFrequencies
                .TryGetValue(token, out double frequency);

            return Math.Log(frequency);
        }

        private double Score(HashSet<string> tokenizedSms, Group group)
        {
            var tokensScore = tokenizedSms.Sum(x => TokenScore(group, x));
            return Math.Log(group.Proportion) + tokensScore;
        }

    }
}
