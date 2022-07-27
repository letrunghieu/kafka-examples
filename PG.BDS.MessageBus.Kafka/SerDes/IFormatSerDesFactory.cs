using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace PG.BDS.MessageBus.Kafka.SerDes
{
    internal interface IFormatSerDesFactory
    {
        public ISerializer<T> MakeSerializer<T>();
    }
}
