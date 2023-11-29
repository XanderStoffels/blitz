using Blitz.Http;
using Blitz.Pipelines;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace Blitz;
public class BlitzHttpServer
{
    public IPAddress Address { get; }
    public int Port { get; }

    private readonly IHttpPipeline _pipeline;
    private readonly ILogger<BlitzHttpServer> _logger;  

    public BlitzHttpServer(IPAddress address, int port, IHttpPipeline pipeline, ILogger<BlitzHttpServer> logger)
    {
        Address = address;
        Port = port;
        _pipeline = pipeline;
        _logger = logger;   
    }

    public async Task RunAsync(CancellationToken cancelToken = default)
    {
        _logger.LogDebug("Binding to {Address}:{Port}", Address, Port);

        var listener = new TcpListener(Address, Port);
        listener.Start();

        _logger.LogInformation("Listening on {Address}:{Port}", Address, Port);   

        while (!cancelToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(cancelToken);
            _logger.LogDebug("Accepted connection from {RemoteEndPoint}", client.Client.RemoteEndPoint);
            _ = HandleTcpClient(client, cancelToken);
        }
        listener.Stop();
    }

    private async Task HandleTcpClient(TcpClient client, CancellationToken cancelToken = default)
    {
        try
        {
            while (client.Connected && !cancelToken.IsCancellationRequested)
            {
                var request = await HttpRequest.From(client.GetStream());

                if (request == null) return;

                var context = new HttpContext
                {
                    TcpClient = client,
                    Request = request
                };
                await _pipeline.InvokeAsync(context, cancelToken);
            }    
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(e.Message);
        }
        finally
        {
            _logger.LogDebug("Closing connection from {RemoteEndPoint}", client.Client.RemoteEndPoint);
            client.Dispose();
        }
    }

    public static BlitzBuilder Create() => new();
  
}
