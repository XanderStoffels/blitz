using System.Text;

namespace Blitz.Http.Responses;
public class OkResponse : HttpResponseBase
{
    private ReadOnlyMemory<byte> body;

    public ReadOnlyMemory<byte> Body { get => body;
        set {
            body = value;
            Headers["Content-Length"] = value.Length.ToString();
        } 
    }

    public OkResponse()
    {
        Body = Array.Empty<byte>();
    }

    public OkResponse(ReadOnlyMemory<byte> body)
    {
        Body = body;
    }

    public OkResponse(string body)
    {
        Body = Encoding.UTF8.GetBytes(body);
    }

    public override async Task Serialize(Stream stream, CancellationToken cancelToken = default)
    {
        await stream.WriteAsync(HttpConstants.OkStatusLine, cancelToken);
        await SerializeHeaders(stream, cancelToken);  

        if (Body.Length > 0)
            await stream.WriteAsync(Body, cancelToken);

        await stream.FlushAsync(cancelToken);
    }
}
