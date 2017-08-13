using System;
using SockIt.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace SockIt.Core
{
    /// <summary>A WebSocket services builder</summary>
    public class WebSocketServicesBuilder : IWebSocketServicesBuilder
    {
        public WebSocketServicesBuilder(IServiceCollection services)
        {
            Services = services;
        }
        
        public IServiceCollection Services { get; }

        public IWebSocketServicesBuilder ConfigureOptions(Action<WebSocketOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        public IWebSocketServicesBuilder SetContextFactory<T>() where T : class, IWebSocketContextFactory
        {
            Services.AddSingleton<IWebSocketContextFactory, T>();
            return this;
        }

        public IWebSocketServicesBuilder SetContextAccessor<T>() where T : class, IWebSocketContextAccessor
        {
            Services.AddSingleton<IWebSocketContextAccessor, T>();
            return this;
        }

        public IWebSocketServicesBuilder SetControllerArgumentBinder<T>() where T : class, IWebSocketControllerArgumentBinder
        {
            Services.AddSingleton<IWebSocketControllerArgumentBinder, T>();
            return this;
        }

        public IWebSocketServicesBuilder SetManager<T>() where T : class, IWebSocketManager
        {
            Services.AddSingleton<IWebSocketManager, T>();
            return this;
        }

        public IWebSocketServicesBuilder SetRouteHandler<T>() where T : class, IWebSocketRouteHandler
        {
            Services.AddSingleton<IWebSocketRouteHandler, T>();
            return this;
        }

        public IWebSocketServicesBuilder SetStore<T>() where T : class, IWebSocketStore
        {
            Services.AddSingleton<IWebSocketStore, T>();
            return this;
        }
    }
}
