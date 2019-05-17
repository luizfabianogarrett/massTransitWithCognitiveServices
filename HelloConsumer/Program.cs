using System;

namespace HelloConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Consummer");
            MyBusiness.MyBusiness b = new MyBusiness.MyBusiness();
            b.Consummer();
            Console.ReadKey();
        }
    }
}
