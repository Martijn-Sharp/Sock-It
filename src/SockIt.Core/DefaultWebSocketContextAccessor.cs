#if NET461
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#elif NETCOREAPP1_1
using System.Threading;
#endif
using SockIt.Abstractions;

namespace SockIt.Core
{
    /// <summary>Default implementation of a websocket context accessor, based on DefaultHttpContextAccessor</summary>
    public class DefaultWebSocketContextAccessor : IWebSocketContextAccessor
    {
#if NET461
        private static readonly string LogicalDataKey = "__WebSocketContext_Current__" + AppDomain.CurrentDomain.Id;

        public WebSocketContext WebSocketContext
        {
            get
            {
                var handle = CallContext.LogicalGetData(LogicalDataKey) as ObjectHandle;
                return handle?.Unwrap() as WebSocketContext;
            }
            set
            {
                CallContext.LogicalSetData(LogicalDataKey, new ObjectHandle(value));
            }
        }
#elif NETCOREAPP1_1
        private AsyncLocal<WebSocketContext> _webSocketContextCurrent = new AsyncLocal<WebSocketContext>();
        public WebSocketContext WebSocketContext
        {
            get
            {
                return _webSocketContextCurrent.Value;
            }
            set
            {
                _webSocketContextCurrent.Value = value;
            }
        }
#endif
    }
}
