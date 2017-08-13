using System.Collections.Generic;
using System.Net.WebSockets;

namespace SockIt.Abstractions
{
    /// <summary>
    /// Abstraction for a class that manages websockets
    /// </summary>
    public interface IWebSocketManager
    {
        /// <summary>Add a websocket to the manager</summary>
        void AddWebSocket(WebSocket webSocket);

        /// <summary>Remove a websocket from the manager</summary>
        void RemoveWebSocket(WebSocket webSocket);

        /// <summary>Get all the websockets associated with the request</summary>
        IEnumerable<WebSocket> GetRequestWebSockets();
    }
}