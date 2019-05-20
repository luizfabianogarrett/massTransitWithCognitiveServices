namespace MyBusiness
{
    public class IntentDigit : IIntent
    { 
        public string Entity { get; set; }
        public string AIm { get { return "Iam a Digit " + Entity; } }

        public IntentDigit()
        {
        }

        public IntentDigit(string entity)
        {
            Entity = entity;
        }
    }
}
