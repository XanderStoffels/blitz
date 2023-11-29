
namespace Blitz.Http.Responses;
internal class StreamResponse : HttpResponseBase
{

    private readonly Stream _inputStream;

    public StreamResponse(Stream stream)
    {
        _inputStream = stream;
    }   

    public override async Task Serialize(Stream stream, CancellationToken cancelToken = default)
    {
        if (_inputStream.CanSeek)
        {
            _inputStream.Seek(0, SeekOrigin.Begin);
        }

        if (_inputStream.Length == 0)
        {
            // Return no content.
            var noContent = new NoContentResponse();
            await noContent.Serialize(stream, cancelToken);
            return;
        }

        // Write status line.
        Headers["Content-Length"] = _inputStream.Length.ToString();
        await stream.WriteAsync(HttpConstants.OkStatusLine, cancelToken);
        await SerializeHeaders(stream, cancelToken);
        await _inputStream.CopyToAsync(stream, cancelToken);
        await stream.FlushAsync(cancelToken);
    }
}
