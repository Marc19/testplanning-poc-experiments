using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Experiments.Core.IKafka;
using Experiments.Core.Messages;
using Newtonsoft.Json;

namespace Experiments.Infrastructure.Producers
{
    public class KafkaProducer : IKafkaProducer
    {
        ProducerConfig _producerConfig;

        public KafkaProducer()
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };
        }

        public void Produce(Message message, string topicName)
        {
            Task.Run(async () =>
            {
                var messageObject = new {
                    messageType = message.GetType().Name,
                    occuredAt = DateTime.Now,
                    payload = message
                };

                string messageJson = JsonConvert.SerializeObject(messageObject);

                Thread.Sleep(2000);

                using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
                {
                    Type type = message.GetType();
                    var t = await producer.ProduceAsync(topicName,
                        new Message<Null, string> { Value = messageJson });

                    //t.Wait();

                }

            });
        }
    }
}
