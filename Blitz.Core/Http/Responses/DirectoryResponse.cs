using System.Text;

namespace Blitz.Http.Responses;
internal class DirectoryResponse(DirectoryInfo dir) : HttpResponseBase
{
    private readonly DirectoryInfo _dir = dir;
    public string MountedOn { get; set; } = string.Empty;

    public override Task Serialize(Stream stream, CancellationToken cancelToken = default)
    {
        // If the dir does not exist, return a 404.
        if (!_dir.Exists)
        {
            var notFound = new NotFoundResponse();
            return notFound.Serialize(stream, cancelToken);
        }

        // List clickable files and directories.
        var filesAndDirs = _dir.GetFileSystemInfos()
            .GroupBy(f => f.GetType())
            .ToDictionary(g => g.Key, g => g.ToList());
        var sb = new StringBuilder();

        // Write HTML Document
        sb.Append($"<!DOCTYPE html><html><head><title>Directory Listing</title><body><h1>Directory Listing for {_dir.Name}</h1>");

        // Directories
        sb.Append("<h2>Directories</h2>");
        sb.Append("<table>");
        foreach (var item in filesAndDirs[typeof(DirectoryInfo)])
            sb.Append($"<tr><td><a href=\"{MountedOn}/{item.Name}\">{item.Name}</a></td><td></td></tr>");

        sb.Append("</table>");

        // Files
        sb.Append("<h2>Files</h2>");
        sb.Append("<table>");
        foreach (var item in filesAndDirs[typeof(FileInfo)])
        {
            var file = (FileInfo)item;
            // Write the file or directory name and size.
            sb.Append($"<tr><td><a href=\"{MountedOn}/{item.Name}\">{item.Name}</a></td><td>({file.Length} bytes)</td></tr>");
        }
        sb.Append("</table>");
        sb.Append("</body></html>");

        var content = sb.ToString();
        var ok = new OkResponse(content);
        return ok.Serialize(stream, cancelToken);
    }
}
