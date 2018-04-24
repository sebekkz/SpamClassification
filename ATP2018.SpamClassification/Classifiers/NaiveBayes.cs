using System;
using System.Collections.Generic;
using System.Linq;

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
                .Select(x => new Tuple<SmsLabel, IEnumerable<HashSet<string>>>(x.Key, x.Select(y => y.Item2)))
                .Select(x => new Tuple<SmsLabel, Group>(x.Item1, this.Analyze(x.Item2, total, classificationTokens)));
                
            groups = new Dictionary<SmsLabel, Group>();

            foreach (var ss in s)
            {
                groups.Add(ss.Item1,ss.Item2);
            }
        }

        private Group Analyze(IEnumerable<HashSet<string>> tokens, int total, string[] classificationTokens)
        {
            var t = tokens.ToArray();
            double propotion = (double)t.Length / total;
            var scoredTokens = classificationTokens.Select(x => new Tuple<string, double>(x, TokenFrequency(x, tokens)));

            var dic = new Dictionary<string, double>();
            foreach(var tt in scoredTokens)
            { 
                dic.Add(tt.Item1, tt.Item2);
            }

            return new Group(propotion, dic);
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
            double frequency = 0;
            group
                .TokenFrequencies
                .TryGetValue(token, out frequency);

            return frequency ==0?0: Math.Log(frequency);
        }

        private double Score(HashSet<string> tokenizedSms, Group group)
        {
            var tokensScore = tokenizedSms.Sum(x => TokenScore(group, x));
            return Math.Log(group.Proportion) + tokensScore;
        }

    }
}
