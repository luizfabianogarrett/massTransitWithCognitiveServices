using System;
using System.Collections.Generic;
using System.Text;

namespace Businness
{
    public class MyBusiness
    {
        public void Publish()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://192.168.254.238:5673/poc_mass_transit"), h =>
                {
                    h.Username("admin");
                    h.Password("admin");
                });

                sbc.ReceiveEndpoint(host, "test_queue", ep =>
                {
                    ep.Handler<MyMessage>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                    });
                });
            });
        }
    }
}
