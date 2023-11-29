namespace Blitz.Http;
public interface IHttpResponse
{
    Task Serialize(Stream stream, CancellationToken cancelToken = default);
}
