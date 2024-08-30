using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHS.EQUIPMENT
{
    public class MessageContext
    {
        [ThreadStatic]
        private static MessageContext s_ThreadContext = null;

        private static readonly MessageContext s_GlobalContext = new MessageContext();

        //
        // 요약:
        //     Gets or sets the namespace uris.
        //
        // 값:
        //     The namespace uris.
        public NamespaceTable NamespaceUris { get; set; }

        //
        // 요약:
        //     Gets or sets the server uris.
        //
        // 값:
        //     The server uris.
        public StringTable ServerUris { get; set; }

        //
        // 요약:
        //     Gets or sets the length of the max array.
        //
        // 값:
        //     The length of the max array.
        public int MaxArrayLength { get; set; }

        //
        // 요약:
        //     Gets or sets the length of the max string.
        //
        // 값:
        //     The length of the max string.
        public int MaxStringLength { get; set; }

        //
        // 요약:
        //     Gets or sets the length of the max byte string.
        //
        // 값:
        //     The length of the max byte string.
        public int MaxByteStringLength { get; set; }

        //
        // 요약:
        //     Gets or sets the size of the max message.
        //
        // 값:
        //     The size of the max message.
        public int MaxMessageSize { get; set; }

        //
        // 요약:
        //     Gets or sets the size of the maximum recursive call depth.
        //
        // 값:
        //     The size of the maximum recursive call depth.
        public int MaxCallDepth { get; set; }

        //
        // 요약:
        //     Gets or sets the factory.
        //
        // 값:
        //     The factory.
        public EncodeableFactory Factory { get; set; }

        //
        // 요약:
        //     The default context for the process (used only during XML serialization).
        public static MessageContext GlobalContext => s_GlobalContext;

        //
        // 요약:
        //     The default context for the thread (used only during XML serialization).
        public static MessageContext ThreadContext
        {
            get
            {
                MessageContext messageContext = s_ThreadContext;
                if (messageContext != null)
                {
                    return messageContext;
                }

                return s_GlobalContext;
            }
            set
            {
                s_ThreadContext = value;
            }
        }

        //
        // 요약:
        //     Initializes a new instance of the UnifiedAutomation.UaBase.MessageContext class.
        public MessageContext()
        {
            MaxArrayLength = 65536;
            MaxStringLength = 16777216;
            MaxByteStringLength = 16777216;
            MaxMessageSize = 16777216;
            MaxCallDepth = 100;
            NamespaceUris = new NamespaceTable();
            ServerUris = new StringTable();
            Factory = EncodeableFactory.GlobalFactory;
        }
        //
        // 요약:
        //     Creates a shallow copy of the given message context.
        //
        // 매개 변수:
        //   messageContext:
        //     The message context.
        public MessageContext(MessageContext messageContext)
        {
            MaxMessageSize = messageContext.MaxMessageSize;
            MaxArrayLength = messageContext.MaxArrayLength;
            MaxStringLength = messageContext.MaxStringLength;
            MaxByteStringLength = messageContext.MaxByteStringLength;
            MaxCallDepth = messageContext.MaxCallDepth;
            NamespaceUris = messageContext.NamespaceUris;
            ServerUris = messageContext.ServerUris;
            Factory = messageContext.Factory;
        }
    }
}