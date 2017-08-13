using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;

namespace SockIt.Core
{
    /// <summary>
    /// An abstract class implementation of the websocketaction result interface, 
    /// which adds a non-async execute result method
    /// </summary>
    public abstract class WebSocketActionResult : IWebSocketActionResult
    {
        public virtual Task ExecuteResultAsync(WebSocketContext context)
        {
            ExecuteResult(context);
            return TaskCache.CompletedTask;
        }

        public virtual void ExecuteResult(WebSocketContext context)
        { 
        }
    }
}
