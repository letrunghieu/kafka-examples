using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace HieuLe.Kafka.Example.Microservice.Shared.Serialization
{
    public interface ISerDesFactory<T>
    {
        public SerDes<T> MakeSerDes(ISchemaRegistryClient schemaRegistryClient, AvroSerializerConfig? config = null);
    }
}
