using SockIt.Abstractions.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace SockIt.Abstractions
{
    public class WebSocketOptions
    {
        /// <summary>Input formatters used to deserialize strings</summary>
        public FormatterCollection<IWebSocketInputFormatter> InputFormatters { get; set; } = new FormatterCollection<IWebSocketInputFormatter>();

        /// <summary>Output formatters used to serialize objects</summary>
        public FormatterCollection<IWebSocketOutputFormatter> OutputFormatters { get; set; } = new FormatterCollection<IWebSocketOutputFormatter>();
    }
}
