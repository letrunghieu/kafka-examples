using System;
using System.Collections.Generic;
using System.Text;

namespace PG.BDS.MessageBus.Transport
{
    public interface ITransport
    {
        void SendMessage<TKey, TValue>(Messages.Message<TKey, TValue> message);
    }
}
