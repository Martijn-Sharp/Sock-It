using Microsoft.Net.Http.Headers;

namespace SockIt.Formatters.Json
{
    /// <summary>JSON specific MediaTypeHeaderValues</summary>
    public static class JsonMediaTypeHeaderValues
    {
        public static readonly MediaTypeHeaderValue ApplicationJson
            = MediaTypeHeaderValue.Parse("application/json").CopyAsReadOnly();

        public static readonly MediaTypeHeaderValue TextJson
            = MediaTypeHeaderValue.Parse("text/json").CopyAsReadOnly();

        public static readonly MediaTypeHeaderValue ApplicationJsonPatch
            = MediaTypeHeaderValue.Parse("application/json-patch+json").CopyAsReadOnly();
    }
}
