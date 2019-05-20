namespace MyBusiness
{
    public class IntentVogal : IIntent
    { 
        public string Entity { get; set; }
        public string AIm { get { return "Iam a Vogal " + Entity; } }

        public IntentVogal()
        {
        }

        public IntentVogal(string entity)
        {
            Entity = entity;
        }

    }
}
