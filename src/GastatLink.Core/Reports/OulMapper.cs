using GastatLink.Core.Hl7;
using System.Globalization;

namespace GastatLink.Core.Reports;

public static class OulMapper
{
    public static GasReport Map(Hl7Message message)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns one <c>^</c> separated part of an Hl7 field, counting from 1.
    /// Returns an empty string if that part doesn't exist.
    /// </summary>
    /// <param name="field">The field text, eg <c>3H080000002027000^Blood gas analysis^JC10</c>.</param>
    /// <param name="number">The part number, starting at 1. Part 2 of the example is <c>Blood gas analysis</c>.</param>
    /// <returns>The trimmed part, or <c>""</c> if missing.</returns>
    private static string Component(string field, int number)
    {
        var parts = field.Split('^', StringSplitOptions.TrimEntries);
        return number >= 1 && number <= parts.Length ? parts[number - 1] : "";
    }

    private static DateTime ParseResultTime(string text)
    {
        return DateTime.ParseExact(text, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }
}
