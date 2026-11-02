using GastatLink.Core.Hl7;

namespace GastatLink.Tests.Hl7;

public class Hl7MessageTests
{
    private const string RealMessage =
    """
    MSH|^~\&|LD|IHE-J^LD|LIS|IHE-J^LIS|20261029162054||OUL^R22^OUL_R22|20261029162054932|P|2.5||||||~ISO IR87||ISO 2022-1994
    PID|||^^^^PI||^^^^^^L^I~^^^^^^L^P|||
    SPM|1|^&&101805||020^Arterial blood^JC10||||||Not specified
    SAC|||101805
    ORC|NW||||||||20261029162054|||
    OBR|1|||3H080000002027000^Blood gas analysis^JC10|||||||O||||020^Arterial blood^JC10
    OBX|1|NM|3H080000002027051^pH^JC10||7.460||7.38-7.46|H|||F||||
    OBX|2|NM|3H080000002027052^pCO2^JC10||28.2|^Torr|32-46|L|||F||||
    OBX|3|NM|3H080000002027053^pO2^JC10||37.0|^Torr|74-108|L|||F||||
    OBX|4|NM|ZD047000002027001^tHb(i)^ZZZZ||15.0|^g/dL|12-18| |||F||||
    OBX|5|NM|ZD005000002027001^BP^ZZZZ||761.0|^Torr|||||F||||
    OBX|6|NM|ZD006000002027001^Temp^ZZZZ||37.0|^ﾟC|||||F||||
    OBX|7|NM|ZD007000002027001^FiO2^ZZZZ||21.0|^%|||||F||||
    OBX|8|NM|3H080000002027054^HCO3-^JC10||19.8|^mmol/L|||||F||||
    OBX|9|NM|ZD011000002027001^BE(ecf)^ZZZZ||-4.0|^mmol/L|||||F||||
    OBX|10|NM|3H080000002027055^base excess^JC10||-2.6|^mmol/L|||||F||||
    OBX|11|NM|ZD012000002027001^BB^ZZZZ||45.4|^mmol/L|||||F||||
    OBX|12|NM|3H080000002027057^total co2^JC10||20.7|^mmol/L|||||F||||
    OBX|13|NM|ZD014000002027001^sO2(est)^ZZZZ||75.0|^%|||||F||||
    OBX|14|NM|ZD025000002027001^pO2(a/A)^ZZZZ||0.31||||||F||||
    OBX|15|NM|ZD029000002027001^ctO2(a)^ZZZZ||15.8|^VOL%|||||F||||
    """;

    [Fact]
    public void Segments_RealMessage_HasTwentyOne()
    {
        var message = new Hl7Message(RealMessage.ReplaceLineEndings("\r" + "\r"));
        Assert.Equal(21, message.Segments.Count);
    }

    [Fact]
    public void Segment_Msh_ReturnsTheMshLine()
    {
        var message = new Hl7Message(RealMessage.ReplaceLineEndings("\r" + "\r"));

        Assert.NotNull(message.Segment("MSH"));
        Assert.Equal("20261029162054932", message.Segment("MSH")?.Field(10));
    }

    [Fact]
    public void SegmentsNamed_Obx_ReturnsAllFifteenInOrder()
    {
        var message = new Hl7Message(RealMessage.ReplaceLineEndings("\r" + "\r"));

        var obxSegments = message.SegmentsNamed("OBX");

        Assert.Equal(15, obxSegments.Count);
        Assert.Equal("1", obxSegments[0].Field(1));
        Assert.Equal("15", obxSegments[14].Field(1));
    }
}
