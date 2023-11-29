using Blitz.Http;

using Route = (string, Blitz.Pipelines.Segments.IPipeSegment);

namespace Blitz.Pipelines.Segments;
public class RouterSegment : IPipeSegment
{

    private readonly List<Route> _routes = [];

    public RouterSegment AddRoute(string path, IPipeSegment segment)
    {
        path = path[1..];
        var segmentIndex = path.IndexOf('/');

        if (segmentIndex > -1)
        {
            // If the path contains a slash, we will add a nested router segment.
            // This will allow us to have nested routes.
            var routerSegment = new RouterSegment();
            routerSegment.AddRoute(path[segmentIndex..], segment);
            _routes.Add((path[..segmentIndex], routerSegment));
            return this;
        }

        _routes.Add((path, segment));
        return this;
    }

    public async ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        // Depending on the path, we will invoke a different segment.
        // If no segment is found, we will invoke the next segment in the pipeline.
        var path = context.CurrentLocation[1..];
        var segmentIndex = path.IndexOf('/');
        var route = segmentIndex > -1 ? path[..segmentIndex] : path;

        foreach (var (routePath, segment) in _routes)
        {
            if (routePath == route)
            {
                segmentIndex  = path.IndexOf(routePath);
                context.CurrentLocation = path[(segmentIndex+routePath.Length)..];
                await segment.InvokeAsync(context, next, cancelToken);
                break;
            }
        }
        await next();  
    }
}
