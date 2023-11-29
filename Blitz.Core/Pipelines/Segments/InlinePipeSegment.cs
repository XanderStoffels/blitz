using Blitz.Http;

namespace Blitz.Pipelines.Segments;
internal class InlinePipeSegment(Func<HttpContext, Func<ValueTask>, CancellationToken, ValueTask> func) : IPipeSegment
{
    private readonly Func<HttpContext, Func<ValueTask>, CancellationToken, ValueTask> _func = func;

    public async ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        await _func(context, next, cancelToken);
    }

    public static implicit operator InlinePipeSegment(Func<HttpContext, Func<ValueTask>, CancellationToken, ValueTask> func)
    {
        return new(func);
    }
}
