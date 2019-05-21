using MassTransit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using GreenPipes;
using System.Threading.Tasks;

namespace MyBusiness
{
    public class MyBusiness
    {

        private IBusControl bus;

        public MyBusiness()
        {
            bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://cbznjkyq:yGeXpmAZJ3cIw8xZ0Wu_5Vz4pCtTefhi@eagle.rmq.cloudamqp.com/cbznjkyq"), h =>
                {
                    h.Username("cbznjkyq");
                    h.Password("yGeXpmAZJ3cIw8xZ0Wu_5Vz4pCtTefhi");
                });
            });

            bus.Start();
        }

        public void Consummer()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://cbznjkyq:yGeXpmAZJ3cIw8xZ0Wu_5Vz4pCtTefhi@eagle.rmq.cloudamqp.com/cbznjkyq"), h =>
                {
                    h.Username("cbznjkyq");
                    h.Password("yGeXpmAZJ3cIw8xZ0Wu_5Vz4pCtTefhi");
                });

                sbc.ReceiveEndpoint(host, "publish.queue", ep =>
                {
                    ep.PrefetchCount = 16;
                    ep.UseMessageRetry(x => x.Interval(3, 10000));
                    ep.Consumer<Consumer>();
                });

                /*sbc.ReceiveEndpoint(host, "send.queue", ep =>
                {

                    ep.PrefetchCount = 16;
                    ep.UseMessageRetry(x => x.Interval(3, 10000));
                    ep.Consumer<Consumer>();

                    ep.Handler<IIntent>(context =>
                    {
                        Random x = new Random((int)DateTime.Now.Ticks);
                        System.Threading.Thread.Sleep(x.Next(500, 1000));

                        return Console.Out.WriteLineAsync($"Received Method Sender: {context.Message.AIm}");
                    });

                });*/


            });

            bus.Start();
        }

        public void Publisher(string text)
        {
            var caracter = text.ToCharArray()[0];

            string pathLuisAi = string.Format("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/69e902e7-f7fb-4b82-beb6-9658929fa922?subscription-key=4830bad0ae5c49edaa71ead353fc3f77&timezoneOffset=0&verbose=true&q={0}", HttpUtility.UrlEncode(caracter.ToString()));

            LuisAiResponse result = null;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(pathLuisAi).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<LuisAiResponse>(content.ReadAsStringAsync().Result);
                    }
                }
            }

            var assemblyName = string.Format("MyBusiness.{0}, MyBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", result.topScoringIntent.intent);
            var type = Type.GetType(assemblyName);
            var instance = Activator.CreateInstance(type, new List<string> { string.Format("{0} Score: {1}", result.query, result.topScoringIntent.score) }.ToArray());

            bus.Publish(instance);

        }

        public void Sender(object obj)
        {

            string rabbitMqQueue = "send.queue";

            Task<ISendEndpoint> sendEndpointTask = bus.GetSendEndpoint(new Uri(string.Concat("rabbitmq://cbznjkyq:yGeXpmAZJ3cIw8xZ0Wu_5Vz4pCtTefhi@eagle.rmq.cloudamqp.com/cbznjkyq", "/", rabbitMqQueue)));

            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            Task sendTask = sendEndpoint.Send(obj);
        }
    }
}
