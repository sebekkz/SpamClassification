using System;
using System.Linq;

namespace ATP2018.SpamClassification
{
    class Program
    {
        static void Main(string[] args)
        {
            var smsCollection = new DataReader($@"{Environment.CurrentDirectory}\Data\SMSSpamCollection").Read();
            var trainingData = smsCollection.Take(4800);
            var verificationData = smsCollection.Skip(4800);

            Console.ReadKey();
        }
    }
}
