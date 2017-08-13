using SockIt.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SockIt.Formatters.Json.Extensions
{
    public class JsonWebSocketOptionsSetup : ConfigureOptions<WebSocketOptions>
    {
        public JsonWebSocketOptionsSetup(IOptions<WebSocketJsonOptions> optionsAccessor) 
            : base(options => ConfigureWebSockets(options, optionsAccessor.Value.SerializerSettings))
        {
        }

        public static void ConfigureWebSockets(WebSocketOptions options, JsonSerializerSettings serializerSettings)
        {
            options.InputFormatters.Add(new WebSocketJsonInputFormatter(serializerSettings));
            options.OutputFormatters.Add(new WebSocketJsonOutputFormatter(serializerSettings));
        }
    }
}
