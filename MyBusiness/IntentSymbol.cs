namespace MyBusiness
{
    public class IntentSymbol : IIntent
    { 
        public string Entity { get; set; }
        public string AIm { get { return "Iam a Symbol " + Entity; } }

        public IntentSymbol()
        {
        }

        public IntentSymbol(string entity)
        {
            Entity = entity;
        }
    }
}
