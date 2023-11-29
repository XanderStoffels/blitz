using System.Text;

namespace Blitz.Http.Responses;

public abstract class HttpResponseBase : IHttpResponse
{
    public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>()
    {
        {"Server", "Blitz" } ,
    };

    public abstract Task Serialize(Stream stream, CancellationToken cancelToken = default);

    internal async Task SerializeHeaders(Stream stream, CancellationToken cancelToken = default)
    {
        // Write Headers
        foreach (var header in Headers)
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes($"{header.Key}: {header.Value}"), cancelToken);
            await stream.WriteAsync(HttpConstants.CRLF, cancelToken);

        }
        await stream.WriteAsync(HttpConstants.CRLF, cancelToken);
    }   

}
