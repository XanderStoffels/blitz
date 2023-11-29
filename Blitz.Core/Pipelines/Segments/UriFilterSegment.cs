using Blitz.Http;

namespace Blitz.Pipelines.Segments;
public class UriFilterSegment : IPipeSegment
{
    private readonly FilterSegment _filter;

    public UriFilterSegment(string mask, IPipeSegment segment)
    {
        _filter = new(ctx => ctx.Request.Url.StartsWith(mask), segment);
    }
    public ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        Console.WriteLine("Uri Filter");
        return _filter.InvokeAsync(context, next, cancelToken);
    }
}
