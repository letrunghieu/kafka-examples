using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Google.Protobuf;
using System;
using System.Linq;

namespace PG.BDS.MessageBus.Kafka.SerDes
{
    internal class SerDesFactory : ISerDesFactory
    {
        private readonly ISchemaRegistryClient _schemaRegistryClient;

        public SerDesFactory(ISchemaRegistryClient schemaRegistryClient)
        {
            _schemaRegistryClient = schemaRegistryClient;
        }

        public IDeserializer<T> MakeKeyDeserializerForTopic<T>(string topic)
        {
            throw new NotImplementedException();
        }

        public ISerializer<T> MakeKeySerializerForTopic<T>(string topic)
        {
            if (typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessage<>)))
            {
                Type protobufSerializerType = typeof(ProtobufSerializer<>);
                Type concreteProtobufSerializerType = protobufSerializerType.MakeGenericType(typeof(T));
                return ((IAsyncSerializer<T>)Activator.CreateInstance(concreteProtobufSerializerType, _schemaRegistryClient)).AsSyncOverAsync();
            }

            return null;

        }

        public IDeserializer<T> MakeValueDeserializerForTopic<T>(string topic)
        {
            throw new NotImplementedException();
        }

        public ISerializer<T> MakeValueSerializerForTopic<T>(string topic)
        {
            if (typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessage<>)))
            {
                Type protobufSerializerType = typeof(ProtobufSerializer<>);
                Type concreteProtobufSerializerType = protobufSerializerType.MakeGenericType(typeof(T));
                return (
                        (IAsyncSerializer<T>)Activator.CreateInstance(concreteProtobufSerializerType, _schemaRegistryClient, null)
                    ).AsSyncOverAsync();
            }

            return null;
        }
    }
}
