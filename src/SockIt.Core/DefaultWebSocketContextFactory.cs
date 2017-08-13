using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Http;

namespace SockIt.Core
{
    /// <summary>Default implementation of a websocket context factory</summary>
    public class DefaultWebSocketContextFactory : IWebSocketContextFactory
    {
        private const int DefaultReceiveBufferSize = 4096;

        public DefaultWebSocketContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
            ReceiveBufferSize = DefaultReceiveBufferSize;
        }

        protected IHttpContextAccessor HttpContextAccessor { get; }

        protected int ReceiveBufferSize { get; set; }

        public async Task<WebSocketContext> CreateAsync(WebSocket webSocket)
        {
            var result = new WebSocketContext { HttpContext = HttpContextAccessor.HttpContext };
            var messageBuilder = new StringBuilder();

            // Read whatever the socket has received till the end of the message has arrived
            bool endOfMessage = false;
            while (!endOfMessage)
            {
                // Create a buffer
                var buffer = new ArraySegment<byte>(new byte[ReceiveBufferSize]);
                var received = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                // Write the buffer to the messagebuilder, trim each message-'chunk' for zero-byte characters
                messageBuilder.Append(Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count).Trim('\0'));
                endOfMessage = received.EndOfMessage || received.CloseStatus != null;

                result.Received = received;
            }

            result.Message = messageBuilder.ToString();
            return result;
        }
    }
}
