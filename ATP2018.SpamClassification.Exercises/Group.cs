namespace ATP2018.SpamClassification
{
    using System;
    using System.Collections.Generic;

    public class Group
    {
        public Group(double proportion, Dictionary<string, double> tokenFrequencies)
        {
            if (tokenFrequencies == null) throw new ArgumentNullException(nameof(tokenFrequencies));

            this.Proportion = proportion;
            this.TokenFrequencies = tokenFrequencies;
        }

        public double Proportion { get; }

        public Dictionary<string, double> TokenFrequencies { get; }
    }
}
