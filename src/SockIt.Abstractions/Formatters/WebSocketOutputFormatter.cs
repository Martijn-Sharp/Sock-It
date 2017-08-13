using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;

namespace SockIt.Abstractions.Formatters
{
    /// <summary>A default implementation of a websocket output formatter</summary>
    public abstract class WebSocketOutputFormatter : IWebSocketOutputFormatter
    {
        /// <summary>Indicates the supported media types for the output formatter</summary>
        /// <remarks>Is not used yet</remarks>
        public MediaTypeCollection SupportedMediaTypes { get; } = new MediaTypeCollection();

        protected virtual bool CanWriteType(Type type)
        {
            return true;
        }

        public virtual bool CanWriteResult(WebSocketOutputFormatterCanWriteContext context)
        {
            if (context.ObjectType == null)
            {
                throw new ArgumentNullException(nameof(context.ObjectType));
            }

            if (SupportedMediaTypes.Count == 0)
            {
                throw new InvalidOperationException("No media types supported");
            }

            if (!CanWriteType(context.ObjectType))
            {
                return false;
            }

            if (!context.ContentType.HasValue)
            {
                // If the desired content type is set to null, then the current formatter can write anything
                // it wants.
                context.ContentType = new StringSegment(SupportedMediaTypes[0]);
                return true;
            }

            // Confirm this formatter supports a more specific media type than requested e.g. OK if "text/*"
            // requested and formatter supports "text/plain". contentType is typically what we got in an Accept
            // header.
            var parsedContentType = new MediaType(context.ContentType.Value);
            for (var i = 0; i < SupportedMediaTypes.Count; i++)
            {
                var supportedMediaType = new MediaType(SupportedMediaTypes[i]);
                if (supportedMediaType.IsSubsetOf(parsedContentType))
                {
                    context.ContentType = new StringSegment(SupportedMediaTypes[i]);
                    return true;
                }
            }

            return false;
        }

        public abstract Task WriteResultAsync(WebSocketContext context, object value, Encoding encoding);
    }
}
