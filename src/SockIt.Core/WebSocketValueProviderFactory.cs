using System.Threading.Tasks;
using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SockIt.Core
{
    public class WebSocketValueProviderFactory : IValueProviderFactory
    {
        private readonly string _parameterName;
        private readonly WebSocketContext _context;

        public WebSocketValueProviderFactory(WebSocketContext context, string parameterName)
        {
            _context = context;
            _parameterName = parameterName;
        }

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            context.ValueProviders.Add(new WebSocketValueProvider(_context, _parameterName));
            return TaskCache.CompletedTask;
        }
    }
}
