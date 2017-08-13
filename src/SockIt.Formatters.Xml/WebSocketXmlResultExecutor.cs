using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SockIt.Abstractions;
using SockIt.Abstractions.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;

namespace SockIt.Formatters.Xml
{
    public class WebSocketXmlResultExecutor
    {
        public WebSocketXmlResultExecutor(IOptions<WebSocketOptions> webSocketOptions)
        {
            OutputFormatters = webSocketOptions.Value.OutputFormatters;
        }

        protected FormatterCollection<IWebSocketOutputFormatter> OutputFormatters { get; set; }

        public virtual Task ExecuteAsync(WebSocketContext context, WebSocketXmlResult result)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            // Get the XML output formatter
            var selectedFormatter = OutputFormatters.OfType<WebSocketXmlOutputFormatter>().FirstOrDefault();
            if (selectedFormatter == null)
            {
                return TaskCache.CompletedTask;
            }

            return selectedFormatter.WriteResultAsync(context, result.Value, Encoding.UTF8);
        }
    }
}
