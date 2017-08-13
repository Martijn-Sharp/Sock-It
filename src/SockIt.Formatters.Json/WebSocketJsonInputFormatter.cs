using System;
using System.IO;
using System.Threading.Tasks;
using SockIt.Abstractions;
using SockIt.Abstractions.Formatters;
using Newtonsoft.Json;

namespace SockIt.Formatters.Json
{
    public class WebSocketJsonInputFormatter : IWebSocketInputFormatter
    {
        public WebSocketJsonInputFormatter(JsonSerializerSettings serializerSettings)
        {
            SerializerSettings = serializerSettings;
        }

        protected JsonSerializerSettings SerializerSettings { get; }

        public async Task<object> ReadAsync(WebSocketContext context, object value, Type modelType)
        {
            using (var reader = new StringReader(value.ToString()))
            {
                var serializer = JsonSerializer.Create(SerializerSettings);
                return serializer.Deserialize(reader, modelType);
            }
        }
    }
}
