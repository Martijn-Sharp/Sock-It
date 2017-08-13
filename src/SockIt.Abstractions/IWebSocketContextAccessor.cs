namespace SockIt.Abstractions
{
    /// <summary>Abstraction to access the websocketcontext</summary>
    public interface IWebSocketContextAccessor
    {
        /// <summary>The websocketcontext associated with the request</summary>
        WebSocketContext WebSocketContext { get; set; }
    }
}
