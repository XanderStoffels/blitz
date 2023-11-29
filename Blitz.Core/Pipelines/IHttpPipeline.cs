using Blitz.Http;
using Blitz.Pipelines.Segments;

namespace Blitz.Pipelines;
public interface IHttpPipeline
{
    Task InvokeAsync(HttpContext context, CancellationToken cancelToken = default);
    IHttpPipeline Use(IPipeSegment segment);
}
