using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace SockIt.Formatters.Json
{
    /// <summary>A JSON result for a websocket action</summary>
    public class WebSocketJsonResult : IWebSocketActionResult
    {
        public WebSocketJsonResult(object value)
        {
            Value = value;
        }

        public object Value { get; protected set; }

        public Task ExecuteResultAsync(WebSocketContext context)
        {
            // Use the executor
            var executor = context.HttpContext.RequestServices.GetRequiredService<WebSocketJsonResultExecutor>();
            var result = executor.ExecuteAsync(context, this);
            return result;
        }
    }
}
