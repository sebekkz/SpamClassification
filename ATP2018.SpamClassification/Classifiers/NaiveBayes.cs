namespace ATP2018.SpamClassification.Classifiers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            var selectedGroups = smses
                .Select(x => new Tuple<SmsLabel, HashSet<string>>(x.Label, this.tokenizer.Tokenize(x.Text)))
                .GroupBy(x => x.Item1)
                .Select(x => new Tuple<SmsLabel, IEnumerable<HashSet<string>>>(x.Key, x.Select(y => y.Item2)))
                .Select(x => new Tuple<SmsLabel, Group>(x.Item1, this.Analyze(x.Item2, smses.Length, classificationTokens)));

            this.groups = new Dictionary<SmsLabel, Group>();

            foreach (var group in selectedGroups)
            {
                this.groups.Add(group.Item1, group.Item2);
            }
        }

        public SmsLabel Classify(string text)
        {
            var tokenizedSms = this.tokenizer.Tokenize(text);

            return this.groups
                .OrderByDescending(x => this.Score(tokenizedSms, x.Value))
                .First()
                .Key;
        }

        private Group Analyze(IEnumerable<HashSet<string>> tokens, int total, string[] classificationTokens)
        {
            var t = tokens.ToArray();
            var propotion = (double)t.Length / total;
            var scoredTokens = classificationTokens.Select(x => new Tuple<string, double>(x, this.TokenFrequency(x, tokens)));

            var frequencies = new Dictionary<string, double>();
            foreach (var scoredToken in scoredTokens)
            {
                frequencies.Add(scoredToken.Item1, scoredToken.Item2);
            }

            return new Group(propotion, frequencies);
        }

        private double TokenFrequency(string classificationToken, IEnumerable<HashSet<string>> tokens)
        {
            var countIn = tokens.Count(x => x.Contains(classificationToken));
            return this.Laplace(countIn, tokens.Count());
        }

        private double Laplace(int count, int total)
        {
            return (double)(count + 1) / (total + 1);
        }

        private double TokenScore(Group group, string token)
        {
            group
                .TokenFrequencies
                .TryGetValue(token, out double frequency);

            return frequency == 0 ? 0 : Math.Log(frequency);
        }

        private double Score(HashSet<string> tokenizedSms, Group group)
        {
            var tokensScore = tokenizedSms.Sum(x => this.TokenScore(group, x));
            return Math.Log(group.Proportion) + tokensScore;
        }
    }
}
