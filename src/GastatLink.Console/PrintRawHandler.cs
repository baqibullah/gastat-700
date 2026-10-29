using GastatLink.Core.Hl7;

namespace GastatLink.Console;

public class PrintRawHandler : IMessageHandler
{
    public async Task<string?> HandleAsync(string message, CancellationToken ct)
    {
        // HL7 segments ends in CR, so we replace that with a new line character so each segment would get a new line. 
        System.Console.WriteLine(message.Replace("\r", "\n"));
        return await Task.FromResult<string?>(null);
    }
}
