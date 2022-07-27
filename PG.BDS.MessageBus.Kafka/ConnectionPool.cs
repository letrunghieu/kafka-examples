using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PG.BDS.MessageBus.Kafka.SerDes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PG.BDS.MessageBus.Kafka
{
    internal class ConnectionPool : IConnectionPool
    {
        private readonly Dictionary<string, IClient> _initializedProducers = new Dictionary<string, IClient>();
        private readonly KafkaOptions _kafkaOptions;
        private readonly ILogger<ConnectionPool> _logger;
        private readonly ISerDesFactory _serDesFactory;

        public ConnectionPool(ILogger<ConnectionPool> logger, IOptions<KafkaOptions> kafkaOptions, ISerDesFactory serDesFactory)
        {
            _logger = logger;
            _kafkaOptions = kafkaOptions.Value;
            _serDesFactory = serDesFactory;
        }

        public IProducer<TKey, TValue> GetProducer<TKey, TValue>(string key)
        {
            if (_initializedProducers.ContainsKey(key))
            {
                return (IProducer<TKey, TValue>)_initializedProducers[key];
            }

            var newProducer = CreateProducer<TKey, TValue>(key);
            _initializedProducers[key] = newProducer;

            return newProducer;
        }

        private IProducer<TKey, TValue> CreateProducer<TKey, TValue>(string topic)
        {
            var config = new ProducerConfig(new Dictionary<string, string>(_kafkaOptions.MainConfig));

            var producer = new ProducerBuilder<TKey, TValue>(config)
                .SetKeySerializer(GetKeySerializer<TKey>(topic))
                .SetValueSerializer(GetValueSerializer<TValue>(topic))
                .Build();

            return producer;
        }

        private ISerializer<T> GetKeySerializer<T>(string topic)
        {
            return _serDesFactory.MakeKeySerializerForTopic<T>(topic);
        }

        private ISerializer<T> GetValueSerializer<T>(string topic)
        {
            return _serDesFactory.MakeValueSerializerForTopic<T>(topic);
        }
    }
}
