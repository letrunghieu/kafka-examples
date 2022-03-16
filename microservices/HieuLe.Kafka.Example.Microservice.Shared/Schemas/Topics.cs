using HieuLe.Kafka.Example.Microservice.Shared.Dtos;
using HieuLe.Kafka.Example.Microservice.Shared.Serialization;
using HieuLe.Kafka.Example.Microservice.Shared.Serialization.Impl;

namespace HieuLe.Kafka.Example.Microservice.Shared.Schemas
{
    public static class Topics
    {
        public static readonly Topic<string, Order> Orders = new(
            "orders",
            new AvroSerDesFactory<string>(),
            new AvroSerDesFactory<Order>()
            );

        public static readonly Topic<string, OrderValidation> OrderValidations = new(
            "order-validations",
            new AvroSerDesFactory<string>(),
            new AvroSerDesFactory<OrderValidation>()
            );

        public record class Topic<TKey, TValue>(
            string Name,
            ISerDesFactory<TKey> KeySerDesFactory,
            ISerDesFactory<TValue> ValueSerDesFactory
            );
    }
}
