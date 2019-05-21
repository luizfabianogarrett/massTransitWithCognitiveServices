using MassTransit;
using System;
using System.Threading.Tasks;

namespace MyBusiness
{
    public class Consumer : IConsumer<IIntent>
    {
        public Task Consume(ConsumeContext<IIntent> context)
        {
            IIntent newCustomer = context.Message;

            Random x = new Random((int)DateTime.Now.Ticks);

            System.Threading.Thread.Sleep(x.Next(500, 1000));

            Console.Out.WriteLineAsync($"Received Method Publisher: {context.Message.AIm}");

            return Task.FromResult(context.Message);
        }
    }
}
