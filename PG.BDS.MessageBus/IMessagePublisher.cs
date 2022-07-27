using System.Threading;
using System.Threading.Tasks;

namespace PG.BDS.MessageBus
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TKey, TValue>(string topicName, TKey key, TValue contentObj, CancellationToken cancellationToken = default);
    }
}
