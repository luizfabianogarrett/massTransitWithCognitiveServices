using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("luis.ai classifique o caracter: ");

            MyBusiness.MyBusiness b = new MyBusiness.MyBusiness();

            do
            {

                var letter = Console.ReadKey().KeyChar.ToString().ToLower();

                b.Publisher(letter);

            } while (true);
        }
    }
}
