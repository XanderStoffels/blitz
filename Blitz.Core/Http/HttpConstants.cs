using System.Text;

namespace Blitz.Http;
internal static class HttpConstants
{
    public static readonly ReadOnlyMemory<byte> OkStatusLine = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n");
    public static readonly ReadOnlyMemory<byte> NoContentStatusLine = Encoding.UTF8.GetBytes("HTTP/1.1 204 No Content\r\n");
    public static readonly ReadOnlyMemory<byte> NotFoundStatusLine = Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n");
    public static readonly ReadOnlyMemory<byte> BadRequestStatusLine = Encoding.UTF8.GetBytes("HTTP/1.1 400 Bad Request\r\n");
    public static readonly ReadOnlyMemory<byte> InternalServerErrorStatusLine = Encoding.UTF8.GetBytes("HTTP/1.1 500 Internal Server Error\r\n");

    public static readonly ReadOnlyMemory<byte> CRLF = Encoding.UTF8.GetBytes("\r\n");
}
