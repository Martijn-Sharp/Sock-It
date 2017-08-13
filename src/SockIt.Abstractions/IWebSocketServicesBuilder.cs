using System;
using Microsoft.Extensions.DependencyInjection;

namespace SockIt.Abstractions
{
    /// <summary>Abstraction used to build the websocket services</summary>
    public interface IWebSocketServicesBuilder
    {
        /// <summary>Accessor for services</summary>
        IServiceCollection Services { get; }

        /// <summary>Configure the websocket options</summary>
        IWebSocketServicesBuilder ConfigureOptions(Action<WebSocketOptions> options);

        /// <summary>Sets the context factory implementation</summary>
        IWebSocketServicesBuilder SetContextFactory<T>() where T : class, IWebSocketContextFactory;

        /// <summary>Sets the context accessor implementation</summary>
        IWebSocketServicesBuilder SetContextAccessor<T>() where T : class, IWebSocketContextAccessor;

        /// <summary>Sets the controller argument binder</summary>
        IWebSocketServicesBuilder SetControllerArgumentBinder<T>() where T: class, IWebSocketControllerArgumentBinder;

        /// <summary>Sets the manager implementation</summary>
        IWebSocketServicesBuilder SetManager<T>() where T : class, IWebSocketManager;

        /// <summary>Sets the route handler implementation</summary>
        IWebSocketServicesBuilder SetRouteHandler<T>() where T : class, IWebSocketRouteHandler;

        /// <summary>Sets the store implementation</summary>
        IWebSocketServicesBuilder SetStore<T>() where T : class, IWebSocketStore;
    }
}