using Microsoft.Net.Http.Headers;

namespace SockIt.Formatters.Xml
{
    /// <summary>XML specific MediaTypeHeaderValues</summary>
    public static class XmlMediaTypeHeaderValues
    {
        public static readonly MediaTypeHeaderValue ApplicationXml
            = MediaTypeHeaderValue.Parse("application/xml").CopyAsReadOnly();

        public static readonly MediaTypeHeaderValue TextXml
            = MediaTypeHeaderValue.Parse("text/xml").CopyAsReadOnly();
    }
}
