using System;
using System.Text;
using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;
using System.Linq;
using SockIt.Abstractions.Formatters;

namespace SockIt.Formatters.Json
{
    public class WebSocketJsonResultExecutor
    {
        public WebSocketJsonResultExecutor(IOptions<WebSocketOptions> webSocketOptions)
        {
            OutputFormatters = webSocketOptions.Value.OutputFormatters;
        }

        protected FormatterCollection<IWebSocketOutputFormatter> OutputFormatters { get; set; }

        public virtual Task ExecuteAsync(WebSocketContext context, WebSocketJsonResult result)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            // Get the JSON output formatter
            var selectedFormatter = OutputFormatters.OfType<WebSocketJsonOutputFormatter>().FirstOrDefault();
            if (selectedFormatter == null)
            {
                return TaskCache.CompletedTask;
            }

            return selectedFormatter.WriteResultAsync(context, result.Value, Encoding.UTF8);
        }
    }
}
