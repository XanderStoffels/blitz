using Blitz.Http;

namespace Blitz.Pipelines.Segments;
public class HttpResponseSegment(IHttpResponse response) : IPipeSegment
{
    public IHttpResponse Response { get; } = response;

    public ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        Console.WriteLine("Http Response Segment");

        context.Response = context.Response;
        return next();
    }

}
