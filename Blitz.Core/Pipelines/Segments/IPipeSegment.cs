using Blitz.Http;

namespace Blitz.Pipelines.Segments;
public interface IPipeSegment
{
    ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default);

}
