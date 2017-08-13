using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SockIt.Abstractions;
using SockIt.Abstractions.Formatters;

namespace SockIt.Formatters.Xml
{
    /// <summary>Output formatter that outputs XML</summary>
    public class WebSocketXmlOutputFormatter : WebSocketOutputFormatter
    {
        public WebSocketXmlOutputFormatter()
        {
            SupportedMediaTypes.Add(XmlMediaTypeHeaderValues.ApplicationXml);
            SupportedMediaTypes.Add(XmlMediaTypeHeaderValues.TextXml);
        }

        public override Task WriteResultAsync(WebSocketContext context, object value, Encoding encoding)
        {
            // The XmlSerializer uses the StringWriter to write to the StringBuilder which in turn sets the context.Response
            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder))
            {
                var serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }

            context.Response = stringBuilder.ToString();
            return Task.CompletedTask;
        }
    }
}
