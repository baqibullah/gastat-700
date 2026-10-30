namespace GastatLink.Core.Hl7;

public class Hl7Message(string text)
{
    public IReadOnlyList<Hl7Segment> Segments { get; }

    public Hl7Segment? Segment(string name)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Hl7Segment> SegmentsNamed(string name)
    {
        throw new NotImplementedException();
    }
}
