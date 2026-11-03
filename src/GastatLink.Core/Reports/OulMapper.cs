using GastatLink.Core.Hl7;
using System.Globalization;

namespace GastatLink.Core.Reports;

public static class OulMapper
{
    public static GasReport Map(Hl7Message message)
    {
        var msh = message.Segment("MSH");
        var pid = message.Segment("PID");
        var spm = message.Segment("SPM");
        var sac = message.Segment("SAC");

        if (msh is null)
        {
            throw new InvalidOperationException("Message has no MSH segment");
        }

        var messageControlId = msh.Field(10);
        var resultTime = ParseResultTime(msh.Field(7));
        var specimenId = sac?.Field(3) ?? "";
        var sampleType = Component(spm?.Field(4) ?? "", 2);

        // PID|||1234567890^^^^PI||^^^^^L^I~John^Smith^^^^^L^P||19741115|M<CR>

        // PatientId defined as this, we take the field at position 3.
        var patientId = Component(pid?.Field(3) ?? "", 1);

        // PatientName is at the position 5: ^^^^^L^I~John^Smith^^^^^L^P
        var patientName = ParsePatientName(pid?.Field(5) ?? "");
        var patientDateOfBirth = ParseBirthDate(pid?.Field(7) ?? "");
        var patientSex = (pid?.Field(8) ?? "").Trim();
        var observations = message.SegmentsNamed("OBX").Select(MapObservation).ToList();

        return new GasReport(
            MessageControlId: messageControlId,
            ResultTime: resultTime,
            SpecimenId: specimenId,
            PatientId: patientId,
            PatientName: patientName,
            DateOfBirth: patientDateOfBirth,
            Sex: patientSex,
            SampleType: sampleType,
            Observations: observations
        );
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

    private static string ParsePatientName(string name)
    {
        // PID-5 holds two versions of the name, separated by "~".
        // The machine only fills the second one:
        // no name:        ^^^^^^L^I~^^^^^^L^P
        // one word:       ^^^^^^L^I~^samii^^^^^L^P      (word goes in part 2)
        // two words:      ^^^^^^L^I~muhammad^sami^^^^^L^P
        // The machine keeps only the first two words. "John Benjamin Wick" arrives as John^Benjamin.

        // No "~" means no second version, so there is no name.
        var versions = name.Split('~');
        if (versions.Length < 2) return "";

        // Take part 1 and part 2 of the second version. Parts 3 and up are markers (L, P), not name.
        var splitName = versions[1];
        var first = Component(splitName, 1);
        var second = Component(splitName, 2);

        // Join only the non-empty parts, so a one-word name has no leading space.
        return string.Join(" ", new[] { first, second }.Where(p => p != ""));
    }

    private static DateOnly? ParseBirthDate(string text)
    {
        if (DateOnly.TryParseExact(text, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }
        return null;
    }

    private static DateTime ParseResultTime(string text)
    {
        return DateTime.ParseExact(text, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }

    private static Observation MapObservation(Hl7Segment obx)
    {
        // Obx line example:
        // OBX|2|NM|3H080000002027052^pCO2^JC10||28.2|^Torr|32-46|L|||F||||
        // Position 3 gives us: 3H080000002027052^pCO2^JC10 we then split by ^
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
                    Unit: Component(obx.Field(6), 2),
                    ReferenceRange: obx.Field(7),
                    Flag: obx.Field(8).Trim()
                    );
    }
}
