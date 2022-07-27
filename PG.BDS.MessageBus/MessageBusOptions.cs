using System;
using System.Collections.Generic;

namespace PG.BDS.MessageBus
{
    public class MessageBusOptions
    {
        public MessageBusOptions()
        {
            Extensions = new List<IMessageBusOptionsExtension>();
        }

        /// <summary>
        /// Topic prefix.
        /// </summary>
        public string? TopicNamePrefix { get; set; }

        internal IList<IMessageBusOptionsExtension> Extensions { get; }

        /// <summary>
        /// Registers an extension that will be executed when building services.
        /// </summary>
        /// <param name="extension"></param>
        public void RegisterExtension(IMessageBusOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }
    }
}
