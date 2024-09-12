using Circus.Shared.Models;

namespace Circus.Services.Abstraction;

public class ProgressMessageHandler(
    HttpMessageHandler innerHandler,
    IProgress<UploadProgress> progress,
    Guid uploadId,
    string fileName)
        : DelegatingHandler(innerHandler)
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var progressContent = new ProgressableStreamContent(request.Content, 4096, progress, uploadId, fileName);
            request.Content = progressContent;
        }

        return await base.SendAsync(request, cancellationToken);
    }
}