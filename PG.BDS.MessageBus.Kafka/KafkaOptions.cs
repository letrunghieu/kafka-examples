using Confluent.Kafka;
using System.Collections.Generic;

namespace PG.BDS.MessageBus.Kafka
{
    public class KafkaOptions
    {
        /// <summary>
        /// librdkafka configuration parameters (refer to https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md).
        /// <para>
        /// Topic configuration parameters are specified via the "default.topic.config" sub-dictionary config parameter.
        /// </para>
        /// </summary>
        public readonly Dictionary<string, string> MainConfig;

        public KafkaOptions()
        {
            MainConfig = new Dictionary<string, string>();
            RetriableErrorCodes = new List<ErrorCode>
            {
                ErrorCode.GroupLoadInProress
            };
        }

        /// <summary>
        /// New retriable error code (refer to https://docs.confluent.io/platform/current/clients/librdkafka/html/rdkafkacpp_8h.html#a4c6b7af48c215724c323c60ea4080dbf)
        /// </summary>
        public IList<ErrorCode> RetriableErrorCodes { get; set; }

        public string SchemaRegistryUrl { get; set; }
    }
}
