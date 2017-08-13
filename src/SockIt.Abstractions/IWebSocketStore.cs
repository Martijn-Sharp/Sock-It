using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SockIt.Abstractions
{
    /// <summary>Abstraction for a store to store websockets</summary>
    public interface IWebSocketStore
    {
        /// <summary>Add a websocket to the store based on a key</summary>
        /// <param name="key">Key</param>
        /// <param name="webSocket">Websocket</param>
        /// <returns>Task</returns>
        Task AddWebSocketAsync(string key, WebSocket webSocket);

        /// <summary>Removes a websocket from the store based on a key</summary>
        /// <param name="key">Key</param>
        /// <param name="webSocket">Websocket</param>
        /// <returns>Task</returns>
        Task RemoveWebSocketAsync(string key, WebSocket webSocket);

        /// <summary>Gets all websockets stored with the same key</summary>
        /// <param name="key">Key</param>
        /// <returns>Task</returns>
        Task<IEnumerable<WebSocket>> GetWebSocketsAsync(string key);
    }
}