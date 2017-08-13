using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Http;

namespace SockIt.Core.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketMiddleware(
            RequestDelegate next, 
            IWebSocketManager webSocketManager,
            IWebSocketContextAccessor webSocketContextAccessor, 
            IWebSocketContextFactory webSocketContextFactory,
            IWebSocketRouteHandler router)
        {
            _next = next;
            WebSocketManager = webSocketManager;
            WebSocketContextAccessor = webSocketContextAccessor;
            WebSocketContextFactory = webSocketContextFactory;
            Router = router;
        }

        protected IWebSocketManager WebSocketManager { get; }

        protected IWebSocketContextAccessor WebSocketContextAccessor { get; }

        protected IWebSocketContextFactory WebSocketContextFactory { get; }

        protected IWebSocketRouteHandler Router { get; }

        public async Task Invoke(HttpContext context)
        {
            // Determine if the request is a websocket-request
            if (context.WebSockets.IsWebSocketRequest)
            {
                // Retrieve the websocket from the context and add it using the manager
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                WebSocketManager.AddWebSocket(webSocket);

                // Handling the websocket happens here (while state is open)
                while (webSocket.State == WebSocketState.Open)
                {
                    // Create a context using the factory
                    var webSocketContext = await WebSocketContextFactory.CreateAsync(webSocket);
                    WebSocketContextAccessor.WebSocketContext = webSocketContext;
                    switch (webSocketContext.Received.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            // Invoke routing
                            await Router.RouteAsync(webSocketContext);

                            // Get a byte-array of the result and send it back to each relevant websocket
                            var data = Encoding.UTF8.GetBytes(webSocketContext.Response);
                            var buffer = new ArraySegment<byte>(data);
                            foreach (var socket in WebSocketManager.GetRequestWebSockets())
                            {
                                if (socket != null && socket.State == WebSocketState.Open)
                                {
                                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                                }
                            }

                            break;
                    }
                }

                // If the websocket has closed, removed it from the manager
                if (webSocket.CloseStatus != null)
                {
                    WebSocketManager.RemoveWebSocket(webSocket);
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
