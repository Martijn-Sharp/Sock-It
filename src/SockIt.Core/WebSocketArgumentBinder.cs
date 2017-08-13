using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;

namespace SockIt.Core
{
    public class WebSocketArgumentBinder : IWebSocketControllerArgumentBinder
    {
        public WebSocketArgumentBinder(IOptions<WebSocketOptions> optionsAccessor, IWebSocketContextAccessor webSocketContextAccessor, IControllerArgumentBinder controllerArgumentBinder)
        {
            ControllerArgumentBinder = controllerArgumentBinder;
            WebSocketContextAccessor = webSocketContextAccessor;
            Options = optionsAccessor.Value;
        }

        protected IControllerArgumentBinder ControllerArgumentBinder { get; }

        protected IWebSocketContextAccessor WebSocketContextAccessor { get; }

        protected WebSocketOptions Options { get; }

        public async Task BindArgumentsAsync(ControllerContext controllerContext, object controller, IDictionary<string, object> arguments)
        {
            await ControllerArgumentBinder.BindArgumentsAsync(controllerContext, controller, arguments);
            if (!arguments.Any())
            {
                return;
            }

            var messageParameter = arguments.FirstOrDefault();
            var parameterType = messageParameter.Value.GetType();
            if (!parameterType.GetTypeInfo().IsValueType && parameterType != typeof(string))
            {
                var webSocketContext = WebSocketContextAccessor.WebSocketContext;
                var result = await Options.InputFormatters.First().ReadAsync(webSocketContext, webSocketContext.Message, parameterType);
                arguments[messageParameter.Key] = result;
            }
        }
    }
}
