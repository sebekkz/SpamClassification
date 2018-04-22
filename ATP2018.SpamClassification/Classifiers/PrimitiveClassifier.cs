namespace ATP2018.SpamClassification.Classifiers
{
    public class PrimitiveSpamClassifier : IClassifier
    {
        private readonly string spamWord;

        public PrimitiveSpamClassifier(string spamWord)
        {
            this.spamWord = spamWord;
        }

        public SmsLabel Classify(string text)
        {
            if (text.Contains(spamWord))
                return SmsLabel.Spam;
            return SmsLabel.Ham;
        }
    }
}
