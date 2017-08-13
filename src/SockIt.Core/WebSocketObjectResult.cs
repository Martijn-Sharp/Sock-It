using System;
using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace SockIt.Core
{
    /// <summary>An object result of a websocket action</summary>
    public class WebSocketObjectResult : WebSocketActionResult
    {
        public WebSocketObjectResult(object value)
        {
            Value = value;
        }

        public object Value { get; protected set; }
        
        public MediaTypeCollection ContentTypes { get; set; }

        public Type DeclaredType { get; set; }

        public override Task ExecuteResultAsync(WebSocketContext context)
        {
            // Use the executor
            var executor = context.HttpContext.RequestServices.GetRequiredService<WebSocketObjectResultExecutor>();
            var result = executor.ExecuteAsync(context, this);
            return result;
        }
    }
}
