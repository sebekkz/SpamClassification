namespace ATP2018.SpamClassification.Classifiers
{
    public interface IClassifier
    {
        SmsLabel Classify(string text);
    }
}