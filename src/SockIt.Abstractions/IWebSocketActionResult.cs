using System.Threading.Tasks;

namespace SockIt.Abstractions
{
    /// <summary>Abstraction for a websocket action result</summary>
    public interface IWebSocketActionResult
    {
        /// <summary>Execute the result</summary>
        Task ExecuteResultAsync(WebSocketContext context);
    }
}