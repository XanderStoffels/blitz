using Blitz.Http;

namespace Blitz.Pipelines.Segments;
public class NoOpSegment : IPipeSegment
{
    public ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        return ValueTask.CompletedTask;
    }
}
