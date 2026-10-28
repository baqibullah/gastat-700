using System.Net;
using System.Net.Sockets;
using System.Text;
using GastatLink.Core.Hl7;
using Microsoft.Extensions.Logging;

namespace GastatLink.Infrastructure;

#pragma warning disable CA1873
public class Hl7Listener(ListenerOptions listenerOptions, IMessageHandler messageHandler, ILogger<Hl7Listener> logger)
{
    private static readonly Encoding ShiftJis;

    // Machine sends text in ShiftJIS which is an old Japenese text encoding.
    static Hl7Listener()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        ShiftJis = Encoding.GetEncoding("shift_jis");
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var endPoint = new IPEndPoint(IPAddress.Parse(listenerOptions.IpAddress), listenerOptions.Port);
        var listener = new TcpListener(endPoint);
        listener.Start();

        logger.LogInformation("Listening on {EndPoint}", endPoint);

        try
        {
            while (!ct.IsCancellationRequested)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(ct);
                _ = HandleConnectionAsync(client, ct);
            }
        }
        catch (OperationCanceledException)
        {
            // Ctrl+C: normal way to stop.
        }
        finally
        {
            listener.Stop();
            logger.LogInformation("Stopped listening");
        }
    }

    private async Task HandleConnectionAsync(TcpClient client, CancellationToken ct)
    {
        var remote = client.Client.RemoteEndPoint;
        logger.LogInformation("Connected from {Remote}", remote);

        var framer = new MllpFramer();
        var buffer = new byte[4096];

        try
        {
            using (client)
            {
                var stream = client.GetStream();
                while (true)
                {
                    int read = await stream.ReadAsync(buffer, ct);
                    if (read == 0) break;
                    foreach (var bytes in framer.Push(buffer.AsSpan(0, read)))
                    {
                        string message = ShiftJis.GetString(bytes);
                        string? reply = await messageHandler.HandleAsync(message, ct);
                        if (reply is not null)
                            await SendAsync(stream, reply, ct);
                    }
                }
            }
            logger.LogInformation("Closed by machine {Remote}", remote);
        }
        catch (IOException ex)
        {
            logger.LogWarning(ex, "Connection {Remote} dropped", remote);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Stopping connection {Remote}", remote);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error on connection {Remote}", remote);
        }
    }

    private static async Task SendAsync(NetworkStream stream, string reply, CancellationToken ct)
    {
        byte[] text = ShiftJis.GetBytes(reply);
        byte[] framed = [.. text, 0x1C, 0x0D];
        await stream.WriteAsync(framed, ct);
    }
}
