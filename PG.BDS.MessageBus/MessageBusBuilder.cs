using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace PG.BDS.MessageBus
{
    /// <summary>
    /// Used to verify cap message queue extension was added on a ServiceCollection
    /// </summary>
    public class MessageBusProviderMakerService
    {
    }

    public sealed class MessageBusBuilder
    {
        public MessageBusBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
