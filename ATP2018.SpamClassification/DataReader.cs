using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ATP2018.SpamClassification
{
    public class DataReader {

        private readonly string path;

        public DataReader(string path)
        {
            this.path = path;
        }

        public IEnumerable<Sms> Read()
        {
            return Read(path);
        }

        private IEnumerable<Sms> Read(string path)
        {
            var lines = File.ReadAllLines(path);
            return lines.Select(line => this.ParseLine(line));
        }

        private Sms ParseLine(string line)
        {
            var splitedLine = line.Split('\t');
            return new Sms(
                this.ParseLabel(splitedLine[0]),
                splitedLine[1]);
        }

        private SmsLabel ParseLabel(string label)
        {
            if (label.Equals("ham", StringComparison.InvariantCultureIgnoreCase))
                return SmsLabel.Ham;
            if (label.Equals("spam", StringComparison.InvariantCultureIgnoreCase))
                return SmsLabel.Spam;
            return SmsLabel.Incorrect;
        }
    }
}
