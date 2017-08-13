using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace SockIt.Abstractions
{
    /// <summary>The context used for the WebSocket implementation</summary>
    public class WebSocketContext
    {
        /// <summary>The HTTP context</summary>
        public HttpContext HttpContext { get; set; }

        /// <summary>The received result of the websocket which invoked the request</summary>
        public WebSocketReceiveResult Received { get; set; }

        /// <summary>The message sent through the socket</summary>
        public string Message { get; set; }

        /// <summary>The response that will be sent back through the socket</summary>
        public string Response { get; set; }
    }
}
