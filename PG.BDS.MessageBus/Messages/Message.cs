using System.Collections.Generic;

namespace PG.BDS.MessageBus.Messages
{
    public class Message<TKey, TValue>
    {
        public Message(TKey key, TValue value, IDictionary<string, string> headers, string destinationTopic)
        {
            Key = key;
            Value = value;
            Headers = headers;
            DestinationTopic = destinationTopic;
        }

        public TKey Key { get; }

        public TValue Value { get; }

        public IDictionary<string, string> Headers { get; }

        public string DestinationTopic { get; }
    }
}
