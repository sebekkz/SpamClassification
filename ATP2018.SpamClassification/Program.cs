﻿namespace ATP2018.SpamClassification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ATP2018.SpamClassification.Classifiers;
    using ATP2018.SpamClassification.VocabularyGenerators;

    public class Program
    {
        public static void Main(string[] args)
        {
            var smsCollection = new DataReader($@"{Environment.CurrentDirectory}\Data\SMSSpamCollection").Read();
            var trainingData = smsCollection.Skip(1000);
            var verificationData = smsCollection.Take(1000);
            ITokenizer tokenizer = new ToLowerWordsTokenizer();
            var classicitor = new NaiveBayes(tokenizer);
            IVocabularyGenerator vacabulary = new TopWordsVocabulary();

            classicitor.Train(trainingData.ToArray(), vacabulary.GetVocabulary(tokenizer, trainingData));
            Evaluate(tokenizer, classicitor, verificationData);

            Console.WriteLine("___________________________________");

            trainingData = smsCollection.Take(4800);
            verificationData = smsCollection.Skip(4800);

            tokenizer = new WordsTokenizer();
            classicitor.Train(trainingData.ToArray(), new string[] { "FREE", "txt", "car", "call", "i", "mobile", "you", "me" });
            Evaluate(tokenizer, classicitor, verificationData);

            Console.ReadKey();
        }

        private static void Evaluate(ITokenizer tokenizer, IClassifier classifier, IEnumerable<Sms> verificationData)
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
