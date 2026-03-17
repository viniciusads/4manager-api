namespace _4Tech._4Manager.Application.Interfaces
{
    public interface IPdfGeneratorService
    {
        Task<byte[]> GenerateFromHtmlAsync(string html, string css, CancellationToken cancellationToken = default);
    }
}
