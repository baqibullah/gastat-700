using GastatLink.Console;
using GastatLink.Infrastructure;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<Hl7Listener>();

var options = new ListenerOptions("192.168.150.20", 5000);
var listener = new Hl7Listener(options, new PrintRawHandler(), logger);

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

await listener.RunAsync(cts.Token);
