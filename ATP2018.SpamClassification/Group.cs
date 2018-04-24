namespace ATP2018.SpamClassification
{
    using System;
    using System.Collections.Generic;

    public class Group
    {
        public Group(double proportion, Dictionary<string, double> tokenFrequencies)
        {
            this.Proportion = proportion;
            this.TokenFrequencies = tokenFrequencies ?? throw new ArgumentNullException(nameof(tokenFrequencies));
        }

        public double Proportion { get; }

        public Dictionary<string, double> TokenFrequencies { get; }
    }
}
