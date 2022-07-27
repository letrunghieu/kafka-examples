using Confluent.Kafka;

namespace PG.BDS.MessageBus.Kafka.SerDes
{
    public enum SerDesFormat
    {
        Avro,
        Protobuf
    }

    internal interface ISerDesFactory
    {
        public ISerializer<T> MakeKeySerializerForTopic<T>(string topic);
        public IDeserializer<T> MakeKeyDeserializerForTopic<T>(string topic);
        public ISerializer<T> MakeValueSerializerForTopic<T>(string topic);
        public IDeserializer<T> MakeValueDeserializerForTopic<T>(string topic);
    }
}
