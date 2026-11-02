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

    private static Observation MapObservation(Hl7Segment obx)
    {
        // Obx line example:
        // OBX|2|NM|3H080000002027052^pCO2^JC10||28.2|^Torr|32-46|L|||F||||
        // Postion 3 gives us: 3H080000002027052^pCO2^JC10 we then split by ^
        // Code: 3H080000002027052
        // Name: pCO2
        // Value: Field(5) = 28.2
        // Unit: Field(6) = ^Torr -> Split -> Torr
        // ReferenceRange: Field(7) = 32-46
        // Flag: Field(8) = L
        return new Observation(
                    Code: Component(obx.Field(3), 1),
                    Name: Component(obx.Field(3), 2),
                    Value: obx.Field(5),
                    Unit: Component(obx.Field(6), 1),
                    ReferenceRange: obx.Field(7),
                    Flag: obx.Field(8).Trim("").ToString()
                    );
    }
}
