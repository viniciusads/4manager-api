using _4Tech._4Manager.Infrastructure.Services;
using FluentAssertions;
using Moq;
using System.Net;

namespace _4Tech._4Manager.Application.Tests.Services
{
    public class PdfGeneratorServiceTest
    {
        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public HttpRequestMessage? CapturedRequest { get; private set; }
            public string? CapturedBody { get; private set; }

            public FakeHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                CapturedRequest = request;

                if (request.Content != null)
                {
                    CapturedBody = await request.Content.ReadAsStringAsync();
                }

                return _response;
            }
        }

        private static PdfGeneratorService CreateService(
            HttpResponseMessage response,
            out FakeHttpMessageHandler handler)
        {
            handler = new FakeHttpMessageHandler(response);

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock
                .Setup(f => f.CreateClient("PdfService"))
                .Returns(httpClient);

            return new PdfGeneratorService(factoryMock.Object);
        }

        [Fact]
        public async Task GenerateFromHtmlAsync_ShouldReturnBytes_WhenRequestIsSuccessful()
        {
            
            var expectedBytes = new byte[] { 1, 2, 3 };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(expectedBytes)
            };

            var service = CreateService(response, out var handler);

            var result = await service.GenerateFromHtmlAsync(
                "<h1>Test</h1>",
                "body { color: red; }"
            );

            result.Should().BeEquivalentTo(expectedBytes);

            handler.CapturedRequest.Should().NotBeNull();
            handler.CapturedRequest!.Method.Should().Be(HttpMethod.Post);
            handler.CapturedRequest.RequestUri!.ToString()
                .Should().Contain("/forms/chromium/convert/html");
        }

        [Fact]
        public async Task GenerateFromHtmlAsync_ShouldThrow_WhenStatusCodeIsNotSuccess()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var service = CreateService(response, out _);

            Func<Task> act = async () =>
                await service.GenerateFromHtmlAsync("html", "css");

            await act.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task GenerateFromHtmlAsync_ShouldSendMultipartFormData_WithHtmlAndCss()
        {
           
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(new byte[] { 0x01 })
            };

            var service = CreateService(response, out var handler);

            var html = "<h1>Hello</h1>";
            var css = "body { background: black; }";

            await service.GenerateFromHtmlAsync(html, css);

            handler.CapturedRequest.Should().NotBeNull();
            handler.CapturedRequest!.Method.Should().Be(HttpMethod.Post);
            handler.CapturedRequest.RequestUri!.ToString()
                .Should().Contain("/forms/chromium/convert/html");

            handler.CapturedBody.Should().NotBeNull();
            handler.CapturedBody.Should().Contain(html);
            handler.CapturedBody.Should().Contain(css);
            handler.CapturedBody.Should().Contain("text/html");
            handler.CapturedBody.Should().Contain("text/css");
        }
    }
}
