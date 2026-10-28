namespace GastatLink.Core.Hl7;

public interface IMessageHandler
{
    // Receive one complete message as text.
    Task<string?> HandleAsync(string message, CancellationToken ct);
}
