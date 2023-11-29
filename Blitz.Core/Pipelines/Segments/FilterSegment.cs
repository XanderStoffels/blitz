using Blitz.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blitz.Pipelines.Segments;
public class FilterSegment(Func<HttpContext, bool> filter, IPipeSegment segment) : IPipeSegment
{
    private readonly Func<HttpContext, bool> _filter = filter;
    private readonly IPipeSegment _segment = segment;

    public async ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        if (_filter(context))
            await _segment.InvokeAsync(context, next, cancelToken);
        else
            await next();
    }
}