namespace GastatLink.Core.Hl7;

public class Hl7Segment
{
    // Using a list here for each segment line to place the parts split by "|"
    private readonly List<string> _segmentParts = [];

    // Describe the three character code for each line: OBX, MSH etc
    // OBX|2|NM|3H080000002027052^pCO2^JC10||28.2|^Torr|32-46|L|||F||||
    public string Name { get; }

    public Hl7Segment(string line)
    {
        // It will contain each segmenet defined like this:
        // ["OBX", "NM", "3H080000002027052^pCO2^JC10", "", "28.2",...]
        _segmentParts.AddRange(line.Split("|"));
        Name = _segmentParts[0];
    }


    // Field 5 of OBX is 28.2
    public string Field(int number)
    {
        return _segmentParts[0];
    }
}
