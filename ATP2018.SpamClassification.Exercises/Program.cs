using ATP2018.SpamClassification.VocabularyGenerators;

namespace ATP2018.SpamClassification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Classifiers;

    public class Program
    {
        public static void Main(string[] args)
        {
            var smsCollection = new DataReader($@"{Environment.CurrentDirectory}\Data\SMSSpamCollection").Read();
            var trainingData = smsCollection.Skip(1000);
            var verificationData = smsCollection.Take(1000);
            var tokenizer = new WordsTokenizer();

            Run("Using HAM classifier ... ", () =>
            {
                var hamClassifier = new HamClassifier();
                Evaluate(hamClassifier, verificationData);
            });

            Run("Using Primitive classifier classifier ... ", () =>
            {
                var primitiveSpamClassifier = new PrimitiveSpamClassifier();
                Evaluate(primitiveSpamClassifier, verificationData);
            });

            Run("Using Primitive Naive Bayer classifier ... ", () =>
            {
                var naiveBayesClassifier = new NaiveBayes(tokenizer);
                IVocabularyGenerator vacabulary = new TopWordsVocabulary();
                naiveBayesClassifier.Train(trainingData.ToArray(), vacabulary.GetVocabulary(tokenizer, trainingData));
                Evaluate(naiveBayesClassifier, verificationData);
            });        

            Console.WriteLine("Press any key to terminate.");
            Console.ReadKey();
        }

        private static void Run(string header, Action action)
        {
            try
            {
                Console.WriteLine(header);
                Console.WriteLine("------------------------");
                action();                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine();
        }

        private static void Evaluate(IClassifier classifier, IEnumerable<Sms> verificationData)
        {
            var valid = verificationData.Average(x => Validate(x.Label, classifier.Classify(x.Text)));
            var validHam = verificationData.Where(x => x.Label == SmsLabel.Ham).Average(x => Validate(x.Label, classifier.Classify(x.Text)));
            var validSpam = verificationData.Where(x => x.Label == SmsLabel.Spam).Average(x => Validate(x.Label, classifier.Classify(x.Text)));
            Console.WriteLine($"Correctly classified: {valid * 100:F2}%");
            Console.WriteLine($"Correctly classified Ham: {validHam * 100:F2}%");
            Console.WriteLine($"Correctly classified Spam: {validSpam * 100:F2}%");
        }

        private static double Validate(SmsLabel value1, SmsLabel value2)
        {
            return value1 == value2 ? 1.0 : 0.0;
        }
    }
}
