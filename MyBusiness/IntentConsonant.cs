namespace MyBusiness
{
    public class IntentConsonant : IIntent
    { 
        public string Entity { get; set; }
        public string AIm { get { return "Iam a Consonant " + Entity; } }

        public IntentConsonant()
        {
        }

        public IntentConsonant(string entity)
        {
            Entity = entity;
        }

    }
}
