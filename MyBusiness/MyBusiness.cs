using MassTransit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

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

                sbc.ReceiveEndpoint(host, "test_queue", ep =>
                {
                    ep.Handler<IntentConsonant>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received consoant: {context.Message.Entity}");
                    });
                    ep.Handler<IntentVogal>(context =>
                    {
                        
                        return Console.Out.WriteLineAsync($"Received vogal: {context.Message.Entity}");
                    });
                    ep.Handler<IntentDigit>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received Digit: {context.Message.Entity}");
                    });
                    ep.Handler<IntentSymbol>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received Symbol: {context.Message.Entity}");
                    });
                    ep.Handler<None>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received None: {context.Message.Entity}");
                    });
                });


                sbc.ReceiveEndpoint(host, "test_queue_error", ep =>
                {
                    ep.Handler<IntentConsonant>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received consoant: {context.Message.Entity}");
                    });
                    ep.Handler<IntentVogal>(context =>
                    {

                        return Console.Out.WriteLineAsync($"Received vogal: {context.Message.Entity}");
                    });
                    ep.Handler<IntentDigit>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received Digit: {context.Message.Entity}");
                    });
                    ep.Handler<IntentSymbol>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received Symbol: {context.Message.Entity}");
                    });
                    ep.Handler<None>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received None: {context.Message.Entity}");
                    });
                });




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
            var instance =  Activator.CreateInstance(type, new List<string> { string.Format("{0} Score: {1}", result.query, result.topScoringIntent.score) }.ToArray());

            bus.Publish(instance);
        }
    }
}
