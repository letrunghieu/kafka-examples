using Microsoft.Extensions.DependencyInjection;
using PG.BDS.MessageBus.Internal;
using System;

namespace PG.BDS.MessageBus
{
    public static class ServiceCollectionExtensions
    {
        public static MessageBusBuilder AddMessageBus(this IServiceCollection services, Action<MessageBusOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddSingleton<IMessagePublisher, MessagePublisher>();

            //Options and extension service
            var options = new MessageBusOptions();
            setupAction(options);

            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.Configure(setupAction);

            return new MessageBusBuilder(services);
        }
    }
}
