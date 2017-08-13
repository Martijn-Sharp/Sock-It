using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace SockIt.Core
{
    public class WebSocketRouteHandler : IWebSocketRouteHandler
    {
        public WebSocketRouteHandler(
            IActionSelectorDecisionTreeProvider decisionTreeProvider, 
            IControllerFactory controllerFactory, 
            ControllerActionInvokerCache cache,
            IWebSocketControllerArgumentBinder argumentBinder,
            IOptions<MvcOptions> optionsAccessor)
        {
            DecisionTree = decisionTreeProvider.DecisionTree;
            ControllerFactory = controllerFactory;
            Cache = cache;
            ArgumentBinder = argumentBinder;
            ValueProviderFactories = optionsAccessor.Value.ValueProviderFactories.ToArray();
        }

        protected IActionSelectionDecisionTree DecisionTree { get; }

        protected IControllerFactory ControllerFactory { get; }

        protected ControllerActionInvokerCache Cache { get; }

        protected IWebSocketControllerArgumentBinder ArgumentBinder { get; }

        protected IReadOnlyList<IValueProviderFactory> ValueProviderFactories { get; }

        public async Task RouteAsync(WebSocketContext context)
        {
            // Create a route-dictionary based on the request path
            var routeDictionary = GetRoute(context.HttpContext.Request.Path);

            // Now that we have a route, find controller-action candidates using the DecisionTree
            var candidates = DecisionTree.Select(routeDictionary);
            if (candidates == null || candidates.Count == 0)
            {
                return;
            }

            // Of all candidates, select the most appropiate
            var actionDescriptor = SelectBestCandidate(candidates, context);
            if (actionDescriptor == null)
            {
                return;
            }

            // Create a routedata object using for the actioncontext
            var routeData = new RouteData();
            foreach (var item in routeDictionary)
            {
                routeData.Values.Add(item.Key, item.Value);
            }

            // Initialize contexts and the controller using the factory
            var actionContext = new ActionContext(context.HttpContext, routeData, actionDescriptor);
            var controllerContext = new ControllerContext(actionContext);
            var cacheEntry = Cache.GetState(controllerContext);
            controllerContext.ValueProviderFactories = new CopyOnWriteList<IValueProviderFactory>(ValueProviderFactories);
            var controller = ControllerFactory.CreateController(controllerContext);
            
            // Determine the amount of parameters, assume the first parameter is used for the websocket-content
            // and the other parameters originate from the query
            var parameters = new Dictionary<string, object>();
            var parameterCount = cacheEntry.ActionMethodExecutor.ActionParameters.Length;
            if (parameterCount > 0)
            {
                var firstParam = cacheEntry.ActionMethodExecutor.ActionParameters.First(x => !context.HttpContext.Request.Query.ContainsKey(x.Name));
                parameters.Add(firstParam.Name, context.Message);
                controllerContext.ValueProviderFactories.Add(new WebSocketValueProviderFactory(context, firstParam.Name));
                if (parameterCount > 1)
                {
                    foreach (var parameter in context.HttpContext.Request.Query)
                    {
                        parameters.Add(parameter.Key, parameter.Value.ToString());
                    }
                }
            }

            await ArgumentBinder.BindArgumentsAsync(controllerContext, controller, parameters);

            // All actions related to the controller will be done in a try-finally clause so that we can always properly release the controller
            try
            {
                var actionResult = await InvokeActionMethodAsync(cacheEntry.ActionMethodExecutor, controller, parameters);
                await InvokeResultAsync(actionResult, context);
            }
            finally
            {
                ControllerFactory.ReleaseController(controllerContext, controller);
            }
        }

        /// <summary>Creates a route-value dictionary based on a path</summary>
        /// <param name="path">A path represented as a string</param>
        /// <returns>A dictionary with the route key/values</returns>
        /// <remarks>A lot of assumptions are made in this method, must find a way to verify the assumptions</remarks>
        private RouteValueDictionary GetRoute(string path)
        {
            var dictionary = new RouteValueDictionary();

            // Split the path on '/' and ignore all empty results
            var splitted = path.Split('/').Where(result => !string.IsNullOrWhiteSpace(result)).ToArray();
            int index = 0;

            // If there are more than 2 entries, assume the first index is the area
            if (splitted.Length > 2)
            {
                dictionary.Add("area", splitted[index]);
                index++;
            }

            // Assume that the current index is the controller and the current index +1 is the action
            dictionary.Add("controller", splitted[index]);
            dictionary.Add("action", splitted[index + 1]);
            return dictionary;
        }

        private async Task<IWebSocketActionResult> InvokeActionMethodAsync(ObjectMethodExecutor executor, object controller, IDictionary<string, object> parameters)
        {
            // Order the parameters used to invoke the action
            var orderedParameters = ControllerActionExecutor.PrepareArguments(parameters, executor);

            // Execute the action
            var resultAsObject = executor.Execute(controller, orderedParameters);

            // If the result is not of type IWebSocketActionResult, create a WebSocketObjectResult with the result
            return resultAsObject as IWebSocketActionResult ?? new WebSocketObjectResult(resultAsObject);
        }

        private async Task InvokeResultAsync(IWebSocketActionResult result, WebSocketContext controllerContext)
        {
            // Execute the result, the result will write the output for the websocket
            await result.ExecuteResultAsync(controllerContext);
        }

        private ActionDescriptor SelectBestCandidate(IReadOnlyList<ActionDescriptor> candidates, WebSocketContext context)
        {
            List<string> parameters = new List<string> {""};
            parameters.AddRange(context.HttpContext.Request.Query.Select(x => x.Key));
            var candidateWithLikeliness = candidates.ToDictionary(x => x, y => CalculateCandidateLikeliness(y, parameters));
            return candidateWithLikeliness.OrderByDescending(x => x.Value).First().Key;
        }

        private double CalculateCandidateLikeliness(ActionDescriptor candidate, IReadOnlyList<string> parameters)
        {
            double likeliness = 0;
            var candidateParametersCount = Convert.ToDouble(candidate.Parameters.Count);
            var parametersCount = Convert.ToDouble(parameters.Count);
            if (candidateParametersCount == parametersCount)
            {
                likeliness = 1;
            }
            else if(candidateParametersCount < parametersCount)
            {
                likeliness = candidateParametersCount / parameters.Count;
            }
            else if (candidateParametersCount > parametersCount)
            {
                likeliness = parameters.Count/ candidateParametersCount;
            }

            var matchingParams = Convert.ToDouble(candidate.Parameters.Count(x => parameters.Contains(x.Name)));
            if (matchingParams == parametersCount)
            {
                likeliness += 1;
            }
            else if (matchingParams < parametersCount)
            {
                likeliness += matchingParams/ parametersCount;
            }
            else if (matchingParams > parameters.Count)
            {
                likeliness += parametersCount / matchingParams;
            }

            return likeliness;
        }
    }
}
