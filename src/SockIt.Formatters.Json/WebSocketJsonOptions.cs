using Newtonsoft.Json;

namespace SockIt.Formatters.Json
{
    public class WebSocketJsonOptions
    {
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings();
    }
}
