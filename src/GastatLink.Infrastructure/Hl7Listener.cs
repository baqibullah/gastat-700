using System.Net.Sockets;
using System.Text;
using GastatLink.Core.Hl7;
using Microsoft.Extensions.Logging;

namespace GastatLink.Infrastructure;

public class Hl7Listener(ListenerOptions listenerOptions, IMessageHandler messageHandler, ILogger<Hl7Listener> logger)
{
    private static readonly Encoding ShiftJis;

    static Hl7Listener()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        ShiftJis = Encoding.GetEncoding("shift_jis");
    }

    public Task RunAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private Task HandleConnectionAsync(TcpClient client, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private async Task SendAsync(NetworkStream stream, string reply, CancellationToken ct)
    {
        byte[] text = ShiftJis.GetBytes(reply);
        byte[] framed = [.. text, 0x1C, 0x0D];
        await stream.WriteAsync(framed, ct);
    }
}
