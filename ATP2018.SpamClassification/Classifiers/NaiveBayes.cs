using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ATP2018.SpamClassification.Classifiers
{
    public class NaiveBayes : IClassifier
    {
        private readonly ITokenizer tokenizer;
        private Dictionary<SmsLabel, Group> groups;

        public NaiveBayes(ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
        }

        public void Train(Sms[] smses, string[] classificationTokens)
        {
            var total = smses.Length;
            var s = smses
                .Select(x => new Tuple<SmsLabel, HashSet<string>>(x.Label, tokenizer.Tokenize(x.Text)))
                .GroupBy(x => x.Item1)
                .Select(x => new Tuple<SmsLabel, IEnumerable<string>>(x.Key, x.SelectMany(y => y.Item2)))
                .Select(x => new Tuple<SmsLabel, Group>(x.Item1, this.Analyze(x.Item2, total, classificationTokens)));
                
            groups = new Dictionary<SmsLabel, Group>();
            foreach (var ss in s)
            {
                groups.Add(ss.Item1,ss.Item2);
            }
        }

        private Group Analyze(IEnumerable<string> tokens, int total, string[] classificationTokens)
        {
            var t = tokens.ToArray();

            throw new NotImplementedException();
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
