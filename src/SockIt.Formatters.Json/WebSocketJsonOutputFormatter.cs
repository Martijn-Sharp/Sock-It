using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SockIt.Abstractions;
using SockIt.Abstractions.Formatters;
using Newtonsoft.Json;

namespace SockIt.Formatters.Json
{
    /// <summary>Output formatter that outputs JSON</summary>
    public class WebSocketJsonOutputFormatter : WebSocketOutputFormatter
    {
        public WebSocketJsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            SupportedMediaTypes.Add(JsonMediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(JsonMediaTypeHeaderValues.TextJson);
            SerializerSettings = serializerSettings;
        }

        protected JsonSerializerSettings SerializerSettings { get; }

        public override Task WriteResultAsync(WebSocketContext context, object value, Encoding encoding)
        {
            // The JsonSerializer uses the StringWriter to write to the StringBuilder which in turn sets the context.Response
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, value);
            }

            context.Response = stringBuilder.ToString();
            return Task.CompletedTask;
        }
    }
}