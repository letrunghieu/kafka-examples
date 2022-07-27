using Microsoft.Extensions.Options;
using PG.BDS.MessageBus.Messages;
using PG.BDS.MessageBus.Transport;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PG.BDS.MessageBus.Internal
{
    internal class MessagePublisher : IMessagePublisher
    {
        private readonly MessageBusOptions _messageBusOptions;
        private readonly ITransport _transport;

        public MessagePublisher(IOptions<MessageBusOptions> messageBusOptions, ITransport transport)
        {
            _messageBusOptions = messageBusOptions.Value;
            _transport = transport;
        }

        public Task PublishAsync<TKey, TValue>(string topicName, TKey key, TValue contentObj, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => Publish(topicName, key, contentObj), cancellationToken);
        }

        private void Publish<TKey, TValue>(string name, TKey key, TValue value)
        {
            var header = new Dictionary<string, string>();

            Publish(name, key, value, header);
        }

        private void Publish<TKey, TValue>(string name, TKey key, TValue value, IDictionary<string, string> headers)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!string.IsNullOrEmpty(_messageBusOptions.TopicNamePrefix))
            {
                name = $"{_messageBusOptions.TopicNamePrefix}.{name}";
            }

            if (!headers.ContainsKey(Headers.MessageId))
            {
                var messageId = Guid.NewGuid().ToString();
                headers.Add(Headers.MessageId, messageId);
            }

            if (!headers.ContainsKey(Headers.CorrelationId))
            {
                headers.Add(Headers.CorrelationId, headers[Headers.MessageId]);
                headers.Add(Headers.CorrelationSequence, 0.ToString());
            }

            headers.Add(Headers.SentTime, DateTimeOffset.Now.ToString());

            _transport.SendMessage(new Message<TKey, TValue>(key, value, headers, name));

            //var message = new Message(headers, value);

            //long? tracingTimestamp = null;
            //try
            //{
            //    tracingTimestamp = TracingBefore(message);

            //    if (Transaction.Value?.DbTransaction == null)
            //    {
            //        var mediumMessage = _storage.StoreMessage(name, message);

            //        TracingAfter(tracingTimestamp, message);

            //        _dispatcher.EnqueueToPublish(mediumMessage);
            //    }
            //    else
            //    {
            //        var transaction = (CapTransactionBase)Transaction.Value;

            //        var mediumMessage = _storage.StoreMessage(name, message, transaction.DbTransaction);

            //        TracingAfter(tracingTimestamp, message);

            //        transaction.AddToSent(mediumMessage);

            //        if (transaction.AutoCommit)
            //        {
            //            transaction.Commit();
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    TracingError(tracingTimestamp, message, e);

            //    throw;
            //}
        }
    }
}
