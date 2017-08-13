using System;
using SockIt.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace SockIt.Formatters.Json.Extensions
{
    /// <summary>Extends the WebSocketServiceBuilder with JSON methods</summary>
    public static class JsonWebSocketServicesBuilderExtensions
    {
        /// <summary>Add JSON support</summary>
        public static IWebSocketServicesBuilder AddJson(this IWebSocketServicesBuilder builder)
        {
            return builder.AddJson(null);
        }

        /// <summary>Add JSON support with options</summary>
        public static IWebSocketServicesBuilder AddJson(
            this IWebSocketServicesBuilder builder,
            Action<WebSocketJsonOptions> setupAction)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<WebSocketOptions>, JsonWebSocketOptionsSetup>());
            builder.Services.TryAddSingleton<WebSocketJsonResultExecutor>();

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }
            
            return builder;
        }
    }
}
