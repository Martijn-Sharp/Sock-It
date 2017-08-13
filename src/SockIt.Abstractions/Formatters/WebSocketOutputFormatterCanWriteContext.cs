using System;
using Microsoft.Extensions.Primitives;

namespace SockIt.Abstractions.Formatters
{
    /// <summary>Context used to determine if an output formatter can write or not</summary>
    public class WebSocketOutputFormatterCanWriteContext
    {
        /// <summary>The content type (eg: "application/json")</summary>
        public StringSegment ContentType { get; set; }

        /// <summary>The type of the object that needs to be written</summary>
        public Type ObjectType { get; set; }
    }
}
