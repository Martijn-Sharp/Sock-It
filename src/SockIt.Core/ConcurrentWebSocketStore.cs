using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;

namespace SockIt.Core
{
    /// <summary>A websocket store that uses concurrent collections to keep track of the websockets</summary>
    public class ConcurrentWebSocketStore: IWebSocketStore
    {
        protected ConcurrentDictionary<string, IList<WebSocket>> WebSockets { get; } = new ConcurrentDictionary<string, IList<WebSocket>>();

        public Task AddWebSocketAsync(string key, WebSocket webSocket)
        {
            var webSockets = GetOrAddWebSockets(key);
            webSockets.Add(webSocket);
            return TaskCache.CompletedTask;
        }

        public Task RemoveWebSocketAsync(string key, WebSocket webSocket)
        {
            var webSockets = GetOrAddWebSockets(key);
            webSockets.Remove(webSocket);
            return TaskCache.CompletedTask;
        }

        public async Task<IEnumerable<WebSocket>> GetWebSocketsAsync(string key)
        {
            return GetOrAddWebSockets(key);
        }

        private IList<WebSocket> GetOrAddWebSockets(string key)
        {

            // Gets the websockets based on a key, if the entry doesn't exist, it'll create a new one
#if NET461
            return WebSockets.GetOrAdd(key, new SynchronizedCollection<WebSocket>());
#elif NETCOREAPP1_1
            return WebSockets.GetOrAdd(key, new List<WebSocket>());
#endif
        }
    }
}
