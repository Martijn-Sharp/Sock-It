using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace SockIt.Formatters.Xml
{
    /// <summary>A XML result for a websocket action</summary>
    public class WebSocketXmlResult : IWebSocketActionResult
    {
        public WebSocketXmlResult(object value)
        {
            Value = value;
        }

        public object Value { get; protected set; }

        public Task ExecuteResultAsync(WebSocketContext context)
        {
            // Use the executor
            var executor = context.HttpContext.RequestServices.GetRequiredService<WebSocketXmlResultExecutor>();
            var result = executor.ExecuteAsync(context, this);
            return result;
        }
    }
}
