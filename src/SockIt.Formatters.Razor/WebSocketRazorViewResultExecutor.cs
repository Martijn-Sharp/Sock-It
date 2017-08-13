using System;
using System.Threading.Tasks;
using SockIt.Abstractions;
using SockIt.Abstractions.Formatters;
using SockIt.Formatters.Razor.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;

namespace SockIt.Formatters.Razor
{
    public class WebSocketRazorViewResultExecutor
    {
        public WebSocketRazorViewResultExecutor(IOptions<WebSocketOptions> webSocketOptions, IRazorEngineService razorEngineService)
        {
            OutputFormatters = webSocketOptions.Value.OutputFormatters;
            RazorEngineService = razorEngineService;
        }

        protected FormatterCollection<IWebSocketOutputFormatter> OutputFormatters { get; set; }

        protected IRazorEngineService RazorEngineService { get; set; }

        public virtual Task ExecuteAsync(WebSocketContext context, WebSocketRazorViewResult result, ActionContext actionContext)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            // Use the Razor Engine service to render the view
            context.Response = RazorEngineService.RenderViewToString(actionContext, result.ViewPath, result.Value);
            return TaskCache.CompletedTask;
        }
    }
}
