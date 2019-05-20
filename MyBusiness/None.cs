namespace MyBusiness
{
    public class None : IIntent
    { 
        public string Entity { get; set; }
        public string AIm { get { return "Iam a thing " + Entity; } }
        
        public None()
        {
        }

        public None(string entity)
        {
            Entity = entity;
        }
    }
}
