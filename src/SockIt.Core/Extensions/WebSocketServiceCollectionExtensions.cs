using SockIt.Abstractions;
using SockIt.Core.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SockIt.Core.Extensions
{
    public static class WebSocketServiceCollectionExtensions
    {
        /// <summary>Adds Martijn's WebSockets solution to your application</summary>
        public static IWebSocketServicesBuilder AddSockIt(this IServiceCollection services)
        {
            // In case this hasn't been done, explicitely add the IHttpContextAccessor implementation
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add known result executors
            AddResultExecutors(services);

            // Initialize the builder, configure the options and set service implementations
            var builder = new WebSocketServicesBuilder(services);
            builder.ConfigureOptions(Configure);
            builder.SetContextFactory<DefaultWebSocketContextFactory>();
            builder.SetContextAccessor<DefaultWebSocketContextAccessor>();
            builder.SetControllerArgumentBinder<WebSocketArgumentBinder>();
            builder.SetManager<DefaultWebSocketManager>();
            builder.SetRouteHandler<WebSocketRouteHandler>();
            builder.SetStore<ConcurrentWebSocketStore>();
            return builder;
        }

        private static void AddResultExecutors(IServiceCollection services)
        {
            services.TryAddSingleton<WebSocketObjectResultExecutor>();
        }

        private static void Configure(WebSocketOptions options)
        {
            options.OutputFormatters.Add(new WebSocketStringOutputFormatter());
        }
    }
}
