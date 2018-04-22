namespace ATP2018.SpamClassification.Classifiers
{
    public class HamClassifier : IClassifier
    {
        public SmsLabel Classify(string text)
        {
            return SmsLabel.Ham;
        }
    }
}
