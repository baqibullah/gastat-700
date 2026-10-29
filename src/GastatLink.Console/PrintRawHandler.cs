using GastatLink.Core.Hl7;

namespace GastatLink.Console;

public class PrintRawHandler : IMessageHandler
{
    public async Task<string?> HandleAsync(string message, CancellationToken ct)
    {
        System.Console.WriteLine(message.Replace("\r", "\n"));
        return await Task.FromResult<string?>(message);
    }
}
