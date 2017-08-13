using System.Text;
using System.Threading.Tasks;

namespace SockIt.Abstractions.Formatters
{
    /// <summary>Abstraction for a websocket-specific output formatter</summary>
    public interface IWebSocketOutputFormatter
    {
        /// <summary>Can this output formatter be used with the given context</summary>
        bool CanWriteResult(WebSocketOutputFormatterCanWriteContext context);

        /// <summary>Write the result to the WebSocketContext</summary>
        /// <param name="context">The websocket context</param>
        /// <param name="value">The object which needs to be written to the result</param>
        /// <param name="encoding">The used encoding</param>
        Task WriteResultAsync(WebSocketContext context, object value, Encoding encoding);
    }
}