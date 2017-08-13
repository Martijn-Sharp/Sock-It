using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SockIt.Formatters.Razor
{
    /// <summary>A Razor view result for a websocket action</summary>
    public class WebSocketRazorViewResult : IWebSocketActionResult
    {
        public WebSocketRazorViewResult(ActionContext actionContext, string viewPath, object value)
        {
            ActionContext = actionContext;
            ViewPath = viewPath;
            Value = value;
        }

        public ActionContext ActionContext { get; protected set; }

        public string ViewPath { get; protected set; }

        public object Value { get; protected set; }

        public Task ExecuteResultAsync(WebSocketContext context)
        {
            // Use the executor
            var executor = context.HttpContext.RequestServices.GetRequiredService<WebSocketRazorViewResultExecutor>();
            var result = executor.ExecuteAsync(context, this, ActionContext);
            return result;
        }
    }
}
