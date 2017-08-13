using System.Threading.Tasks;

namespace SockIt.Abstractions
{
    /// <summary>Abstraction to handle routes</summary>
    public interface IWebSocketRouteHandler
    {
        /// <summary>Route the request</summary>
        Task RouteAsync(WebSocketContext context);
    }
}