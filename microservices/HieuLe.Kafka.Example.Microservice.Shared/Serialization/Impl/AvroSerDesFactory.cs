using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace HieuLe.Kafka.Example.Microservice.Shared.Serialization.Impl
{
    internal class AvroSerDesFactory<T> : ISerDesFactory<T>
    {
        public SerDes<T> MakeSerDes(ISchemaRegistryClient schemaRegistryClient, AvroSerializerConfig? config = null)
        {
            return new SerDes<T>(
                new AvroSerializer<T>(schemaRegistryClient, config).AsSyncOverAsync(),
                new AvroDeserializer<T>(schemaRegistryClient, config).AsSyncOverAsync()
            );
        }
    }
}
