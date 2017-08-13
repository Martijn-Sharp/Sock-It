using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Http;

namespace SockIt.Core
{
    /// <summary>Default implementation of a websocket manager</summary>
    public class DefaultWebSocketManager : IWebSocketManager
    {
        public DefaultWebSocketManager(IHttpContextAccessor httpContextAccessor, IWebSocketStore webSocketStore)
        {
            HttpContextAccessor = httpContextAccessor;
            WebSocketStore = webSocketStore;
        }

        protected IHttpContextAccessor HttpContextAccessor { get; }

        protected IWebSocketStore WebSocketStore { get; }

        protected ConcurrentDictionary<string, List<WebSocket>> WebSockets { get; } = new ConcurrentDictionary<string, List<WebSocket>>();

        public void AddWebSocket(WebSocket webSocket)
        {
            // Add the websocket to the store
            WebSocketStore.AddWebSocketAsync(GetWebSocketKey(), webSocket);
        }

        public void RemoveWebSocket(WebSocket webSocket)
        {
            // Remove the websocket from the store
            WebSocketStore.RemoveWebSocketAsync(GetWebSocketKey(), webSocket);
        }

        public IEnumerable<WebSocket> GetRequestWebSockets()
        {
            // Get the websockets from the store
            return WebSocketStore.GetWebSocketsAsync(GetWebSocketKey()).Result;
        }

        private string GetWebSocketKey()
        {
            // Get a reference to the request object
            var request = HttpContextAccessor.HttpContext.Request;

            // Build a key by combining the request path and request query
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(request.Path);
            stringBuilder.Append(request.QueryString);

            // Return it lowered and trimmed
            return stringBuilder.ToString().ToLower().Trim();
        }
    }
}
