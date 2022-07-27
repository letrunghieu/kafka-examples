using System;
using System.Collections.Generic;
using System.Text;

namespace PG.BDS.MessageBus.Messages
{
    public static class Headers
    {
        public const string MessageId = "message-id";

        public const string CorrelationId = "correlation-id";

        public const string CorrelationSequence = "correlation-id-sequence";

        public const string SentTime = "sent-time";
    }
}
