using GastatLink.Core.Hl7;
using GastatLink.Core.Reports;

namespace GastatLink.Console;

public class PrintRawHandler : IMessageHandler
{
    public async Task<string?> HandleAsync(string message, CancellationToken ct)
    {
        var gasReport = OulMapper.Map(new Hl7Message(message));

        System.Console.WriteLine("<--------------GASTAT REPORT------------->\n");
        System.Console.WriteLine(
        $"ID: {gasReport.MessageControlId} | Time: {gasReport.ResultTime} | Specimen: {gasReport.SpecimenId} | " +
        $"Patient: {gasReport.PatientId} | Name: {gasReport.PatientName} | DOB: {gasReport.DateOfBirth} | " +
        $"Sex: {gasReport.Sex} | Sample: {gasReport.SampleType}");

        System.Console.WriteLine("<------------OBSERVATIONS------------->");
        foreach (var obs in gasReport.Observations)
        {
            System.Console.WriteLine($"{obs.Name}: {obs.Value} {obs.Unit} ({obs.ReferenceRange}) {obs.Flag}");
        }

        return await Task.FromResult<string?>(null);
    }
}
