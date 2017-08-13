using Microsoft.AspNetCore.Mvc;

namespace SockIt.Formatters.Razor.Service
{
    /// <summary>Abstraction for a razor engine service</summary>
    public interface IRazorEngineService
    {
        /// <summary>Render a view</summary>
        /// <param name="viewPath">Path to the view</param>
        /// <param name="model">Model used in the view</param>
        /// <returns>Rendered view as a string</returns>
        string RenderViewToString(string viewPath, object model);

        /// <summary>Render a view</summary>
        /// <param name="actionContext">The actioncontext to be used</param>
        /// <param name="viewPath">Path to the view</param>
        /// <param name="model">Model used in the view</param>
        /// <returns>Rendered view as a string</returns>
        string RenderViewToString(ActionContext actionContext, string viewPath, object model);
    }
}