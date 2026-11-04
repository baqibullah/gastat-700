using GastatLink.Core.Reports;

namespace GastatLink.Core.Hl7;

public class OulHandler(IReportDisplay display) : IMessageHandler
{
    public Task<string?> HandleAsync(string message, CancellationToken ct)
    {
        var hl7Message = new Hl7Message(message);

        if (hl7Message.Segment("MSH")?.Field(9).Split("^")[0] != "OUL")
        {
            return Task.FromResult<string?>(null);
        }

        var gasReport = OulMapper.Map(hl7Message);
        display.Show(gasReport);

        return Task.FromResult<string?>(AckBuilder.Build(hl7Message, DateTime.Now));
    }
}
