using System;
using System.Threading.Tasks;

namespace SockIt.Abstractions.Formatters
{
    public interface IWebSocketInputFormatter
    {
        Task<object> ReadAsync(WebSocketContext context, object value, Type modelType);
    }
}