using Blitz.Http;
using Blitz.Http.Responses;
using System.Web;

namespace Blitz.Pipelines.Segments;
public class FileSystemSegment : IPipeSegment
{
    private readonly DirectoryInfo _dir;
    private readonly string _mountedOn;
    public FileSystemSegment(DirectoryInfo dir, string mountedOn)
    {
        if (!dir.Exists)
            throw new ArgumentException("Directory does not exist.", nameof(dir));
        
        _dir = dir;
        _mountedOn = mountedOn;
    }

    public ValueTask InvokeAsync(HttpContext context, Func<ValueTask> next, CancellationToken cancelToken = default)
    {
        if (string.IsNullOrWhiteSpace(context.CurrentLocation) || context.CurrentLocation == "/")
        {
            var index = Path.Combine(_dir.FullName, "index.html");
            if (File.Exists(index))
            {
                var file = new FileInfo(index);
                context.Response = new FileStreamResponse(file);
                return next();
            }
            context.Response = new DirectoryResponse(_dir) {  MountedOn = _mountedOn};
            return next();
        }

        // Decode from URL.
        var resource = context.CurrentLocation[1..];
        resource = HttpUtility.UrlDecode(resource);

        var path = Path.Combine(_dir.FullName, resource);
        
        // If the resource is empty and a index.html file exists, return the index.html file.
        if (string.IsNullOrWhiteSpace(resource))
        {
            var index = Path.Combine(path, "index.html");
            if (File.Exists(index))
            {
                var file = new FileInfo(index);
                context.Response = new FileStreamResponse(file);
                return next();
            }
        }

        // If the resource is a directory, return a directory listing.
        if (Directory.Exists(path))
        {
            var dir = new DirectoryInfo(path);

            // Check if the directory is the same as the root directory.
            // If it is not, pass the directory name as a prefix to the directory listing.
            if (dir.FullName != _dir.FullName)
            {
                context.Response = new DirectoryResponse(dir) { MountedOn = Path.Combine(_mountedOn, resource) };
                return next();
            }

            context.Response = new DirectoryResponse(dir);
            return next();
        }

        // If the resource is a file, return the file.
        if (File.Exists(path))
        {
            var file = new FileInfo(path);
            context.Response = new FileStreamResponse(file);
            return next();
        }

        // Maybe the pasth exists, but it's a html file.
        // If it is, return the html file.
        var maybeHtml = $"{path}.html";
        if (File.Exists(maybeHtml))
        {
            var file = new FileInfo(maybeHtml);
            context.Response = new FileStreamResponse(file);
            return next();
        }

        // If the resource is not found, go next.
        return next();
    }
}
