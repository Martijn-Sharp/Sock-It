using System;
using System.Text;
using System.Threading.Tasks;
using SockIt.Abstractions.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace SockIt.Abstractions.Test.Formatters
{
    public class WebSocketOutputFormatterTest
    {
        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(WebSocketOutputFormatterCanWriteContext))]
        public void CanWriteResult_ReturnsFalseForNonStringTypes(Type type)
        {
            var outputFormatter = new TestWebSocketOutputFormatter { SupportedMediaTypes = { "" } };
            Assert.False(outputFormatter.CanWriteResult(new WebSocketOutputFormatterCanWriteContext { ObjectType = type }));
        }

        [Fact]
        public void CanWriteResult_ReturnsTrueForString()
        {
            var outputFormatter = new TestWebSocketOutputFormatter { SupportedMediaTypes = { "" } };
            Assert.True(outputFormatter.CanWriteResult(new WebSocketOutputFormatterCanWriteContext { ObjectType = typeof(string) }));
        }

        [Fact]
        public void CanWriteResult_ThrowsExceptionForNoSupportedMediaTypes()
        {
            var outputFormatter = new TestWebSocketOutputFormatter();
            Exception ex = Assert.Throws<InvalidOperationException>(() =>outputFormatter.CanWriteResult(new WebSocketOutputFormatterCanWriteContext
            {
                ObjectType = typeof(string)
            }));

            Assert.Equal("No media types supported", ex.Message);
        }
        
        public static TheoryData<MediaTypeHeaderValue> MediaTypeCollection => new TheoryData<MediaTypeHeaderValue>
        {
            MediaTypeHeaderValue.Parse("text/json").CopyAsReadOnly(),
            MediaTypeHeaderValue.Parse("text/test").CopyAsReadOnly()
        };

        [Theory]
        [MemberData(nameof(MediaTypeCollection))]
        public void CanWriteResult_ReturnsTrueWithSupportedMediaType(MediaTypeHeaderValue mediaType)
        {
            var outputFormatter = new TestWebSocketOutputFormatter { SupportedMediaTypes = { mediaType } };
            Assert.True(outputFormatter.CanWriteResult(new WebSocketOutputFormatterCanWriteContext { ObjectType = typeof(string), ContentType = new StringSegment(mediaType.MediaType)}));
        }

        [Fact]
        public void CanWriteResult_ReturnsFalseWithUnsupportedMediaType()
        {
            var outputFormatter = new TestWebSocketOutputFormatter { SupportedMediaTypes = { MediaTypeHeaderValue.Parse("text/json").CopyAsReadOnly() } };
            Assert.False(outputFormatter.CanWriteResult(new WebSocketOutputFormatterCanWriteContext { ObjectType = typeof(string) , ContentType = new StringSegment("test")}));
        }
    }

    public class TestWebSocketOutputFormatter : WebSocketOutputFormatter
    {
        protected override bool CanWriteType(Type type)
        {
            return type == typeof(string);
        }
        
        public override Task WriteResultAsync(WebSocketContext context, object value, Encoding encoding)
        {
            throw new System.NotImplementedException();
        }
    }
}
