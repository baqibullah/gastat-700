using GastatLink.Core.Hl7;

namespace GastatLink.Core.Reports;

public static class OulMapper
{
    public static GasReport Map(Hl7Message message)
    {
        throw new NotImplementedException();
    }

    private static string SplitSegmentFields(string field, int number)
    {
        var parts = field.Split('^', StringSplitOptions.TrimEntries);
        return number >= 1 && number <= parts.Length ? parts[number - 1] : "";
    }
}
