using ATP2018.SpamClassification.Classifiers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATP2018.SpamClassification
{
    class Program
    {
        static void Main(string[] args)
        {
            var smsCollection = new DataReader($@"{Environment.CurrentDirectory}\Data\SMSSpamCollection").Read();
            var trainingData = smsCollection.Skip(1000);
            var verificationData = smsCollection.Take(1000);
            var tokenizer = new ToLowerWordsTokenizer();
            var classicitor = new NaiveBayes(tokenizer);

            classicitor.Train(trainingData.ToArray(), new string [] { "free", "txt", "car", "call", "i", "mobile", "you", "me"});
            Evaluate(tokenizer, classicitor, verificationData);

            Console.ReadKey();
        }

        private static void Evaluate(ITokenizer tokenizer, IClassifier classifier, IEnumerable<Sms> verificationData)
        {
            var valid = verificationData.Average(x => Validate(x.Label, classifier.Classify(x.Text)));
            var validHam = verificationData.Where(x=> x.Label == SmsLabel.Ham).Average(x => Validate(x.Label, classifier.Classify(x.Text)));
            var validSpam = verificationData.Where(x => x.Label == SmsLabel.Spam).Average(x => Validate(x.Label, classifier.Classify(x.Text)));
            Console.WriteLine($"Correctly classified: {valid * 100}%");
            Console.WriteLine($"Correctly classified Ham: {validHam * 100}%");
            Console.WriteLine($"Correctly classified Spam: {validSpam * 100}%");
        }

        private static double Validate(SmsLabel value1, SmsLabel value2)
        {
            return value1 == value2 ? 1.0 : 0.0;
        }
    }
}
