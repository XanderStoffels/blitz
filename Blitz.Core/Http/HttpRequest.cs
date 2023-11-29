namespace Blitz.Http;
public class HttpRequest
{
    public required HttpMethod Method { get; set; }
    public required Dictionary<string, string> Headers { get; set; }
    public required string Protocol { get; set; }
    public required string Url { get; init; }


    public static async Task<HttpRequest?> From(Stream stream)
    {
        if (!stream.CanRead) return null;

        StreamReader reader = new(stream);
        string? line = await reader.ReadLineAsync();
        if (line == null) return null;

        HttpMethod? method = line switch
        {
            string s when s.StartsWith("GET") => HttpMethod.GET,
            string s when s.StartsWith("POST") => HttpMethod.POST,
            string s when s.StartsWith("PUT") => HttpMethod.PUT,
            string s when s.StartsWith("DELETE") => HttpMethod.DELETE,
            string s when s.StartsWith("PATCH") => HttpMethod.PATCH,
            string s when s.StartsWith("HEAD") => HttpMethod.HEAD,
            string s when s.StartsWith("OPTIONS") => HttpMethod.OPTIONS,
            _ => null
        };

        if (method is null) return null;

        var resource = line.Split(' ')[1];
        var protocol = line.Split(' ')[2];

        // Parse Headers
        var headers = new Dictionary<string, string>();
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line == string.Empty) break;
            var header = line.Split(':', 2);
            headers.Add(header[0], header[1]);
        }


        return new()
        {
            Method = method.Value,
            Headers = headers,
            Url = resource,
            Protocol = protocol
        };

    }
}
