namespace GastatLink.Core.Hl7;

public class Hl7Message(string message)
{
    public IReadOnlyList<Hl7Segment> Segments { get; } =
     [.. message.Split("\r", StringSplitOptions.RemoveEmptyEntries).Select(line => new Hl7Segment(line))];

    public Hl7Segment? Segment(string name)
    {
        return Segments.FirstOrDefault(segment => segment.Name == name);
    }

    public IReadOnlyList<Hl7Segment> SegmentsNamed(string name)
    {
        return [.. Segments.Where(segment => segment.Name == name)];
    }
}
