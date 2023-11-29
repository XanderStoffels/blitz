using Blitz.Http;
using Blitz.Pipelines.Segments;
using System.Diagnostics;

namespace Blitz.Pipelines;
public class HttpPipeline : IHttpPipeline
{
    private readonly List<IPipeSegment> _segments = [];
    private Func<HttpContext, CancellationToken, ValueTask>? _buildPipeline;

    public async Task InvokeAsync(HttpContext context, CancellationToken cancelToken = default)
    {
        if (_segments.Count == 0)
            return;

        if (_segments.Count == 1)
        {
            await _segments[0].InvokeAsync(context, () => ValueTask.CompletedTask, cancelToken);
            return;
        }

        if (_buildPipeline == null)
            BuildPipeline();

        Debug.Assert(_buildPipeline != null);
        await _buildPipeline.Invoke(context, cancelToken);
    }

    public IHttpPipeline Use(IPipeSegment segment)
    {
        _segments.Add(segment);
        return this;
    }

    public IHttpPipeline Use(Func<HttpContext, Func<ValueTask>, CancellationToken, ValueTask> func)
    {
        _segments.Add(new InlinePipeSegment(func));
        return this;
    }

    private void BuildPipeline()
    {
        var sequence = () => ValueTask.CompletedTask;
        _buildPipeline = async (ctx, cancelToken) =>
        {
            for (var i = _segments.Count - 1; i >= 0; i--)
            {
                var segment = _segments[i];
                var next = sequence;
                sequence = async () => await segment.InvokeAsync(ctx, next, cancelToken);
            }
            await sequence.Invoke();
        };
    }
}
