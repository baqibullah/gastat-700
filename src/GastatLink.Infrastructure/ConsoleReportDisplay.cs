using GastatLink.Core.Reports;

namespace GastatLink.Infrastructure;

public class ConsoleReportDisplay : IReportDisplay
{
    public void Show(GasReport report)
    {
        Console.WriteLine();
        Console.WriteLine("=== GAS REPORT ===");
        Console.WriteLine($"Time:      {report.ResultTime:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"Specimen:  {OrDash(report.SpecimenId)}");
        Console.WriteLine($"Patient:   {OrDash(report.PatientId)}");
        Console.WriteLine($"Name:      {OrDash(report.PatientName)}");
        Console.WriteLine($"DOB:       {report.DateOfBirth?.ToString("yyyy-MM-dd") ?? "-"}");
        Console.WriteLine($"Sex:       {SexText(report.Sex)}");
        Console.WriteLine($"Sample:    {OrDash(report.SampleType)}");
        Console.WriteLine();

        Console.WriteLine($"{"Parameter",-14}{"Value",-9}{"Unit",-10}{"Range",-10}Flag");
        Console.WriteLine(new string('-', 49));

        foreach (var obs in report.Observations)
        {
            Console.WriteLine(
                $"{obs.Name,-14}{obs.Value,-9}{UnitText(obs.Unit),-10}{obs.ReferenceRange,-10}{obs.Flag}");
        }
    }

    private static string OrDash(string text)
    {
        return text == "" ? "-" : text;
    }

    private static string SexText(string sex)
    {
        return sex switch
        {
            "M" => "Male",
            "F" => "Female",
            "O" => "Other",
            "U" => "Unknown",
            _ => "-"
        };
    }

    private static string UnitText(string unit)
    {
        return unit == "\uFF9FC" ? "°C" : unit;
    }
}