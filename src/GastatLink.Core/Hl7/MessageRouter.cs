using GastatLink.Core.Reports;

namespace GastatLink.Core.Hl7;

public class MessageRouter(IReportDisplay display) : IMessageHandler
{
    public Task<string?> HandleAsync(string message, CancellationToken ct)
    {
        var hl7Message = new Hl7Message(message);
        var messageType = hl7Message.Segment("MSH")?.Field(9).Split('^')[0];

        return messageType switch
        {
            "OUL" => BuildAck(hl7Message),
            "QBP" => BuildRsp(hl7Message),
            _ => Task.FromResult<string?>(null)
        };
    }

    private static Task<string?> BuildRsp(Hl7Message message)
    {
        return Task.FromResult<string?>(RspBuilder.Build(message, DateTime.Now));
    }

    private Task<string?> BuildAck(Hl7Message message)
    {
        var gasReport = OulMapper.Map(message);
        display.Show(gasReport);
        return Task.FromResult<string?>(AckBuilder.Build(message, DateTime.Now));
    }
}
