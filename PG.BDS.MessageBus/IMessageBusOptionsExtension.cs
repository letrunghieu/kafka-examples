using Microsoft.Extensions.DependencyInjection;

namespace PG.BDS.MessageBus
{
    public interface IMessageBusOptionsExtension
    {
        /// <summary>
        /// Registered child service.
        /// </summary>
        /// <param name="services">add service to the <see cref="IServiceCollection" /></param>
        void AddServices(IServiceCollection services);
    }
}
