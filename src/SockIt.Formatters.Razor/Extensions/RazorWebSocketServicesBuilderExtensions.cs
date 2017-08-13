using SockIt.Abstractions;
using SockIt.Formatters.Razor.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SockIt.Formatters.Razor.Extensions
{
    /// <summary>Extends the WebSocketServiceBuilder with Razor methods</summary>
    public static class RazorWebSocketServicesBuilderExtensions
    {
        /// <summary>Add Razor support</summary>
        public static IWebSocketServicesBuilder AddRazor(this IWebSocketServicesBuilder builder)
        {
            builder.Services.TryAddSingleton<WebSocketRazorViewResultExecutor>();
            builder.Services.TryAddSingleton<IRazorEngineService, RazorEngineService>();
            return builder;
        }
    }
}
