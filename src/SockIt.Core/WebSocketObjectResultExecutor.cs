using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SockIt.Abstractions;
using SockIt.Abstractions.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace SockIt.Core
{
    public class WebSocketObjectResultExecutor
    {
        public WebSocketObjectResultExecutor(IOptions<WebSocketOptions> webSocketOptions)
        {
            OutputFormatters = webSocketOptions.Value.OutputFormatters;
        }

        protected FormatterCollection<IWebSocketOutputFormatter> OutputFormatters { get; set; }

        public virtual Task ExecuteAsync(WebSocketContext context, WebSocketObjectResult result)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            
            // Select all the formatters that can be used
            var writeContext = new WebSocketOutputFormatterCanWriteContext
            {
                ContentType = new StringSegment(),
                ObjectType = result.Value.GetType()
            };

            var selectedFormatters = OutputFormatters.Where(x => x.CanWriteResult(writeContext));

            if (!selectedFormatters.Any())
            {
                return TaskCache.CompletedTask;
            }
            
            // Select the first and write the result
            // TODO: Figure out a better way to determine the right formatter?
            return selectedFormatters.First().WriteResultAsync(context, result.Value, Encoding.UTF8);
        }
    }
}
