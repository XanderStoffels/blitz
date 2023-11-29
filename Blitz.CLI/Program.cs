using Blitz;
using Blitz.Http;
using Blitz.Http.Responses;
using Blitz.Pipelines.Segments;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
     //.MinimumLevel.Override("Blitz", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// Create a logger for BlitzHttpServer

var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);
var blitzLogger = loggerFactory.CreateLogger<BlitzHttpServer>();

await BlitzHttpServer.Create()
    .Bind(IPAddress.Any, 80)
    .WithLogger(blitzLogger)
    .Use(new UriFilterSegment("/test", new HttpResponseSegment(new OkResponse())))
    .Use(new SendContextResponseSegment())
    .Build()
    .RunAsync();