using Confluent.Kafka;

namespace HieuLe.Kafka.Example.Microservice.Shared.Serialization
{
    public record class SerDes<T>(ISerializer<T> Serializer, IDeserializer<T> Deserializer);
}
