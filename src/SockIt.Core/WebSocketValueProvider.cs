using SockIt.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SockIt.Core
{
    public class WebSocketValueProvider : IValueProvider
    {
        private readonly string _parameterName;
        private readonly WebSocketContext _context;

        public WebSocketValueProvider(WebSocketContext context, string parameterName)
        {
            _context = context;
            _parameterName = parameterName;
        }

        public bool ContainsPrefix(string prefix)
        {
            return true;
        }

        public ValueProviderResult GetValue(string key)
        {
            if (_parameterName == key)
            {
                return new ValueProviderResult(_context.Message);
            }
            else
            {
                return ValueProviderResult.None;
            }
        }
    }
}
