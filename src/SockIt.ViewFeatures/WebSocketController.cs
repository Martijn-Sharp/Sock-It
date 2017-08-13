using SockIt.Formatters.Json;
using SockIt.Formatters.Xml;
using SockIt.Formatters.Razor;
using Microsoft.AspNetCore.Mvc;

namespace SockIt.ViewFeatures
{
    /// <summary>A WebSocket controller</summary>
    public abstract class WebSocketController : ControllerBase
    {
        /// <summary>Create a JSON response</summary>
        public WebSocketJsonResult Json(object value)
        {
            return new WebSocketJsonResult(value);
        }

        /// <summary>Create an XML response</summary>
        public WebSocketXmlResult Xml(object value)
        {
            return new WebSocketXmlResult(value);
        }

        /// <summary>Create a Razor view response</summary>
        public WebSocketRazorViewResult View(object value)
        {
            return View(ControllerContext.ActionDescriptor.ActionName, value);
        }

        /// <summary>Create a Razor view response</summary>
        public WebSocketRazorViewResult View(string viewPath, object value)
        {
            return new WebSocketRazorViewResult(ControllerContext, viewPath, value);
        }
    }
}
