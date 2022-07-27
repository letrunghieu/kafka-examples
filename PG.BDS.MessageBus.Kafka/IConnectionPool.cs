using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace PG.BDS.MessageBus.Kafka
{
    internal interface IConnectionPool
    {
        IProducer<TKey, TValue> GetProducer<TKey, TValue>(string key);
    }
}
