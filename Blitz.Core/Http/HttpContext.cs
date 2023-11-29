using Blitz.Http.Responses;
using System.Net.Sockets;

namespace Blitz.Http;

public class HttpContext : IDisposable
{

    public required TcpClient TcpClient { get; init; }
    public required HttpRequest Request { get; init; }
    public IHttpResponse Response { get; set; } = new NotFoundResponse();
    public Dictionary<string, string> MetaData { get; } = [];

    internal string CurrentLocation
    {
        get
        {
            if (MetaData.TryGetValue("CurrentLocation", out string? location))
                return location;
            
            return Request.Url;
        }
        set
        {
            MetaData["CurrentLocation"] = value;
        }
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        TcpClient.Dispose();
    }

}
