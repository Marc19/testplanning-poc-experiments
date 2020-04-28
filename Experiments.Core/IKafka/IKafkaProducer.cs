using System;
using Experiments.Core.Messages;

namespace Experiments.Core.IKafka
{
    public interface IKafkaProducer
    {
        void Produce(Message message, string topicName);
    }
}
