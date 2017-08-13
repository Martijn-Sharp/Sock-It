using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SockIt.Abstractions
{
    /// <summary>An abstraction for a factory that creates a websocketcontext</summary>
    public interface IWebSocketContextFactory
    {
        /// <summary>Create a websocketcontext based on a websocket</summary>
        Task<WebSocketContext> CreateAsync(WebSocket webSocket);
    }
}