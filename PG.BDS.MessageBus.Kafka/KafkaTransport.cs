using PG.BDS.MessageBus.Messages;
using PG.BDS.MessageBus.Transport;
using System.Collections.Generic;

namespace PG.BDS.MessageBus.Kafka
{
    internal class KafkaTransport : ITransport
    {
        private readonly IConnectionPool _connectionPool;

        public KafkaTransport(IConnectionPool connectionPool)
        {
            _connectionPool = connectionPool;
        }

        public void SendMessage<TKey, TValue>(Message<TKey, TValue> message)
        {
            var producer = _connectionPool.GetProducer<TKey, TValue>(message.DestinationTopic);

            producer.Produce(
                message.DestinationTopic,
                new Confluent.Kafka.Message<TKey, TValue>
                {
                    Key = message.Key,
                    Value = message.Value,
                    Headers = GetKafkaMessageHeaders(message.Headers)
                }
                );
        }

        private Confluent.Kafka.Headers GetKafkaMessageHeaders(IDictionary<string, string> headers)
        {
            var kafkaHeaders = new Confluent.Kafka.Headers();
            foreach (var kv in headers)
            {
                kafkaHeaders.Add(kv.Key, System.Text.Encoding.UTF8.GetBytes(kv.Value));
            }

            return kafkaHeaders;
        }
    }
}
