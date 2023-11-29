
namespace Blitz.Http.Responses;
internal class NoContentResponse : HttpResponseBase
{
    public override async Task Serialize(Stream stream, CancellationToken cancelToken = default)
    {
        Headers.Add("Content-Length", "0");
        await stream.WriteAsync(HttpConstants.NoContentStatusLine, cancelToken);
        await SerializeHeaders(stream, cancelToken);
        await stream.FlushAsync(cancelToken);
    }
}
