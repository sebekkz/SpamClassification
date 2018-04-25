using System.Collections.Generic;
using System.Linq;

namespace ATP2018.SpamClassification.VocabularyGenerators
{
    public class TopWordsVocabulary : IVocabularyGenerator
    {
        public string[] GetVocabulary(ITokenizer tokenizer, IEnumerable<Sms> smses)
        {
            var spamSmsmes = smses
                .Where(x => x.Label == SmsLabel.Spam);

            var spamWords = spamSmsmes
                .SelectMany(x => tokenizer.Tokenize(x.Text)).Distinct().ToArray();

            var tokenizedSpamSmses = spamSmsmes.Select((x => tokenizer.Tokenize(x.Text))).ToArray();

            var topSpamWords = new HashSet<string>(spamWords.OrderByDescending(x => tokenizedSpamSmses.Count(y => y.Contains(x) )).Take(30));


            var hamSmsmes = smses
                .Where(x => x.Label == SmsLabel.Ham);

            var hamWords = hamSmsmes
                .SelectMany(x => tokenizer.Tokenize(x.Text)).Distinct().ToArray();

            var tokenizedHamSmses = hamSmsmes.Select((x => tokenizer.Tokenize(x.Text))).ToArray();

            var topHamWords = new HashSet<string> (hamWords.OrderByDescending(x => tokenizedHamSmses.Count(y => y.Contains(x))).Take(30));

            var intersection = new HashSet<string>(topHamWords);

            intersection.IntersectWith(topSpamWords);

            topHamWords.ExceptWith(intersection);
            topSpamWords.ExceptWith(intersection);

            return topSpamWords.Take(10).Union(topHamWords.Take(10)).ToArray();
        }
    }
}
