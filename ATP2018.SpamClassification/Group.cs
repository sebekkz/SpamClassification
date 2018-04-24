using System;
using System.Collections.Generic;

namespace ATP2018.SpamClassification
{
    public class Group
    {
        public double Proportion { get; }

        public Dictionary<string, double> TokenFrequencies { get; }

        public Group(double proportion, Dictionary<string, double> tokenFrequencies)
        {
            Proportion = proportion;
            TokenFrequencies = tokenFrequencies ?? throw new ArgumentNullException(nameof(tokenFrequencies));
        }
    }
}
