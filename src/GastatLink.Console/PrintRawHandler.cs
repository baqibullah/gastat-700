using GastatLink.Core.Hl7;
using GastatLink.Core.Reports;
using GastatLink.Infrastructure;

namespace GastatLink.Console;

public class PrintRawHandler : IMessageHandler
{
    public async Task<string?> HandleAsync(string message, CancellationToken ct)
    {
        var gasReport = OulMapper.Map(new Hl7Message(message));
        new ConsoleReportDisplay().Show(gasReport);
        return await Task.FromResult<string?>(null);
    }
}
