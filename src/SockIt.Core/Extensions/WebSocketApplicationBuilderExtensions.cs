using SockIt.Core.Middleware;
using Microsoft.AspNetCore.Builder;

namespace SockIt.Core.Extensions
{
    public static class WebSocketApplicationBuilderExtensions
    {
        /// <summary>Use Martijn's WebSockets solution in your request pipeline</summary>
        public static IApplicationBuilder UseSockIt(this IApplicationBuilder applicationBuilder)
        {
            // Use MVC's websocket support
            applicationBuilder.UseWebSockets();

            // Add the websocket middle
            applicationBuilder.UseMiddleware<WebSocketMiddleware>();
            return applicationBuilder;
        }
    }
}
