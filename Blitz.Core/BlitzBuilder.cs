using Blitz.Http;
using Blitz.Pipelines;
using Blitz.Pipelines.Segments;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace Blitz;
public class BlitzBuilder
{
    private readonly HttpPipeline _pipeline = new();
    private int _port = 80;
    private IPAddress _address = IPAddress.Any;
    private ILogger<BlitzHttpServer> _logger = new NullLogger<BlitzHttpServer>();


    public BlitzBuilder Use(IPipeSegment segment)
    {
        _pipeline.Use(segment);
        return this;
    }
    
    public BlitzBuilder Use(Func<HttpContext, Func<ValueTask>, CancellationToken, ValueTask> func)
    {
        _pipeline.Use(func);
        return this;
    }   

    public BlitzBuilder WithLogger(ILogger<BlitzHttpServer> logger)
    {
        _logger = logger;
        return this;
    }   

    public BlitzBuilder Bind(IPAddress address, int port)
    {
        _address = address;
        _port = port;
        return this;
    }

    public BlitzHttpServer Build()
    {
        return new BlitzHttpServer(_address, _port, _pipeline, _logger);
    }

}
