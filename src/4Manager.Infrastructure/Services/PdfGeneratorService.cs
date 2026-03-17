using _4Tech._4Manager.Application.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;

namespace _4Tech._4Manager.Infrastructure.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        private readonly HttpClient _client;

        public PdfGeneratorService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("PdfService");
        }
        public async Task<byte[]> GenerateFromHtmlAsync(string html, string css, CancellationToken cancellationToken = default)
        {
            using var form = new MultipartFormDataContent();

            var htmlContent = new StringContent(html);
            htmlContent.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            var cssContent = new StringContent(css);
            cssContent.Headers.ContentType = new MediaTypeHeaderValue("text/css");

            form.Add(htmlContent, "index.html", "index.html");
            form.Add(cssContent, "style.css", "style.css");

            var response = await _client.PostAsync(
                "/forms/chromium/convert/html",
                form,
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
