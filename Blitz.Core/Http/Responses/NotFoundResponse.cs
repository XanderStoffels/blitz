using System.Text;

namespace Blitz.Http.Responses;
public sealed class NotFoundResponse : HttpResponseBase
{
    public override async Task Serialize(Stream stream, CancellationToken cancelToken = default)
    {
        await stream.WriteAsync(HttpConstants.NotFoundStatusLine, cancelToken);
        await SerializeHeaders(stream, cancelToken);
        await stream.FlushAsync(cancelToken);
    }
}
