using Circus.Shared.Models;
using System.Net;

namespace Circus.Services.Abstraction;

public class ProgressableStreamContent : HttpContent
{
    private const int DefaultBufferSize = 4096;
    private readonly HttpContent _content;
    private readonly int _bufferSize;
    private readonly IProgress<UploadProgress> _progress;
    private readonly Guid _uploadId;
    private readonly string _fileName;

    public ProgressableStreamContent(HttpContent content, int bufferSize, IProgress<UploadProgress> progress, Guid uploadId, string fileName)
    {
        _content = content ?? throw new ArgumentNullException(nameof(content));
        _bufferSize = bufferSize;
        _progress = progress ?? throw new ArgumentNullException(nameof(progress));
        _uploadId = uploadId;
        _fileName = fileName;

        foreach (var header in _content.Headers)
        {
            Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
        var buffer = new byte[_bufferSize];
        TryComputeLength(out var size);
        var uploaded = 0L;

        await using var contentStream = await _content.ReadAsStreamAsync();
        while (true)
        {
            var length = await contentStream.ReadAsync(buffer, 0, buffer.Length);
            if (length <= 0) break;

            uploaded += length;
            _progress.Report(new UploadProgress
            {
                UploadId = _uploadId,
                FileName = _fileName,
                Percentage = (uploaded * 1d) / (size * 1d) * 100
            });
            await stream.WriteAsync(buffer, 0, length);
            await stream.FlushAsync();
        }
    }

    protected override bool TryComputeLength(out long length)
    {
        length = _content.Headers.ContentLength ?? -1;
        return length != -1;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _content.Dispose();
        }
        base.Dispose(disposing);
    }
}
