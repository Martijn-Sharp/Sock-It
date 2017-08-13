using SockIt.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SockIt.Formatters.Xml.Extensions
{
    /// <summary>Extends the WebSocketServiceBuilder with XML methods</summary>
    public static class XmlWebSocketServicesBuilderExtensions
    {
        /// <summary>Add XML support</summary>
        public static IWebSocketServicesBuilder AddXml(this IWebSocketServicesBuilder builder)
        {
            builder.ConfigureOptions(x => x.OutputFormatters.Add(new WebSocketXmlOutputFormatter()));
            builder.Services.TryAddSingleton<WebSocketXmlResultExecutor>();
            return builder;
        }
    }
}
