using Blitz.Http;

namespace Blitz.Pipelines.Segments;
public class SendContextResponseSegment : IPipeSegment
{
    public async ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        Console.WriteLine("Response from HttpContext");
        await context.Response.Serialize(context.TcpClient.GetStream(), cancelToken);
        await next();
    }
}
