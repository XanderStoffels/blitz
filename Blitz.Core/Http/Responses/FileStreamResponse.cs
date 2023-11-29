

namespace Blitz.Http.Responses;
public class FileStreamResponse : HttpResponseBase
{
    private readonly FileInfo _file;
    public FileStreamResponse(FileInfo file)
    {
        _file = file;
    }

    public override async Task Serialize(Stream stream, CancellationToken cancelToken = default)
    {
        if (!_file.Exists)
        {
            var notFound = new NotFoundResponse();
            await notFound.Serialize(stream, cancelToken);
            return;
        }
           
        using var inputStream = _file.OpenRead();
        var streamResponse = new StreamResponse(inputStream);  
        await streamResponse.Serialize(stream, cancelToken);
    }
}
