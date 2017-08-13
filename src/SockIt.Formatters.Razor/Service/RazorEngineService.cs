using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace SockIt.Formatters.Razor.Service
{
    /// <summary>Razor Engine service to render views</summary>
    public class RazorEngineService : IRazorEngineService
    {
        public RazorEngineService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider services)
        {
            ViewEngine = viewEngine;
            TempDataProvider = tempDataProvider;
            Services = services;
        }

        protected IRazorViewEngine ViewEngine { get; }

        protected ITempDataProvider TempDataProvider { get; }

        protected IServiceProvider Services { get; }

        public string RenderViewToString(string viewPath, object model)
        {
            // Create an actioncontext
            var actionContext = GetActionContext();
            return RenderViewToString(actionContext, viewPath, model);
        }

        public string RenderViewToString(ActionContext actionContext, string viewPath, object model)
        {
            // Find the view
            var viewEngineResult = ViewEngine.FindView(actionContext, viewPath, false);
            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find view '{viewPath}'");
            }

            // Use a stringwriter to write the output
            using (var output = new StringWriter())
            {
                // Create the context to render the view with
                var viewContext = new ViewContext(
                    actionContext,
                    viewEngineResult.View,
                    new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model },
                    new TempDataDictionary(actionContext.HttpContext, TempDataProvider),
                    output,
                    new HtmlHelperOptions());

                viewEngineResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext { RequestServices = Services };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
