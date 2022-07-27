using System;

namespace PG.BDS.MessageBus.Kafka
{
    public static class MessageBusOptionsExtensions
    {
        public static MessageBusOptions UseKafka(this MessageBusOptions options, Action<KafkaOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new KafkaMessageBusOptionsExtension(configure));

            return options;
        }
    }
}
