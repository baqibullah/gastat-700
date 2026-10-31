using GastatLink.Core.Hl7;

namespace GastatLink.Tests.Hl7;

public class Hl7SegmentTests
{
    private const string ObxLine = "OBX|2|NM|3H080000002027052^pCO2^JC10||28.2|^Torr|32-46|L|||F||||";
    private const string MshLine =
    "MSH|^~\\&|LD|IHE-J^LD|LIS|IHE-J^LIS|20261029162054||OUL^R22^OUL_R22|20261029162054932|P|2.5||||||~ISO IR87||ISO 2022-1994";

    [Fact]
    public void Field_NormalObxLine_ReturnsValueAtThatNumber()
    {
        var segment = new Hl7Segment(ObxLine);
        Assert.Equal("28.2", segment.Field(5));
    }

    [Fact]
    public void Field_EmptyField_ReturnsEmptyString()
    {
        var segment = new Hl7Segment(ObxLine);
        Assert.Equal("", segment.Field(4));
    }

    [Fact]
    public void Field_NumberPastTheEnd_ReturnsEmptyString()
    {
        var segment = new Hl7Segment(ObxLine);
        Assert.Equal("", segment.Field(40));
    }

    [Fact]
    public void Field_MshLine_UsesProtocolDocumentNumbering()
    {
        var segment = new Hl7Segment(MshLine);
        Assert.Equal("OUL^R22^OUL_R22", segment.Field(9));
        Assert.Equal("20261029162054932", segment.Field(10));
    }
}
