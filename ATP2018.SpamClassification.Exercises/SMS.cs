namespace ATP2018.SpamClassification
{
    public struct Sms
    {
        public readonly SmsLabel Label;
        public readonly string Text;

        public Sms(SmsLabel label, string text)
        {
            this.Label = label;
            this.Text = text;
        }
    }
}
