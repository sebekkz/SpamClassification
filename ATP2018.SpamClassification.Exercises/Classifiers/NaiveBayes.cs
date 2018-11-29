namespace ATP2018.SpamClassification.Classifiers
{
    using System;

    public class NaiveBayes : IClassifier
    {
        private readonly ITokenizer tokenizer;

        public NaiveBayes(ITokenizer tokenizer)
        {
            if (tokenizer == null) throw new ArgumentNullException(nameof(tokenizer));
            this.tokenizer = tokenizer;
        }

        public SmsLabel Classify(string text)
        {
            // TODO: Implement me please
            throw new NotImplementedException();
        }

        public void Train(Sms[] smses, string[] classificationTokens)
        {
            // TODO: Implement me please
            throw new NotImplementedException();
        }
    }
}
