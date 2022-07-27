using Confluent.SchemaRegistry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PG.BDS.MessageBus.Kafka.SerDes;
using PG.BDS.MessageBus.Transport;
using System;

namespace PG.BDS.MessageBus.Kafka
{
    internal sealed class KafkaMessageBusOptionsExtension : IMessageBusOptionsExtension
    {
        private readonly Action<KafkaOptions> _configure;

        public KafkaMessageBusOptionsExtension(Action<KafkaOptions> configure)
        {
            this._configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<MessageBusProviderMakerService>();

            services.Configure(_configure);

            services.AddSingleton<ISerDesFactory, SerDesFactory>();
            services.AddSingleton<IConnectionPool, ConnectionPool>();
            services.AddSingleton<ITransport, KafkaTransport>();
            services.AddSingleton<ISchemaRegistryClient>(services =>
            {
                KafkaOptions kafkaOptions = services.GetRequiredService<IOptions<KafkaOptions>>().Value;
                return new CachedSchemaRegistryClient(new SchemaRegistryConfig
                {
                    Url = kafkaOptions.SchemaRegistryUrl,
                });
            });
        }
    }
}