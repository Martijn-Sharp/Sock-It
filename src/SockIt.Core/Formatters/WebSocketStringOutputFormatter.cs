using System.Text;
using System.Threading.Tasks;
using SockIt.Abstractions;
using SockIt.Abstractions.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;

namespace SockIt.Core.Formatters
{
    /// <summary>String output formatter implementation that outputs the string representation of the value</summary>
    public class WebSocketStringOutputFormatter : IWebSocketOutputFormatter
    {
        public bool CanWriteResult(WebSocketOutputFormatterCanWriteContext context)
        {
            return context.ObjectType == typeof(string);
        }

        public Task WriteResultAsync(WebSocketContext context, object value, Encoding encoding)
        {
            context.Response = value.ToString();
            return TaskCache.CompletedTask;
        }
    }
}
