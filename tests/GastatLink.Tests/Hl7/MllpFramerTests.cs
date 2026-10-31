using System.Text;
using GastatLink.Core.Hl7;

namespace GastatLink.Tests.Hl7;

public class MllpFramerTests
{
    [Fact]
    public void Push_OneCompleteMessage_ReturnsIt()
    {
        byte[] text = Encoding.ASCII.GetBytes("AAA");
        byte[] input = [.. text, 0x1C, 0x0D];

        var mllpFramer = new MllpFramer();
        var result = mllpFramer.Push(input);

        Assert.Single(result);
        Assert.Equal(text, result[0]);
    }

    [Fact]
    public void Push_NoEndMarker_ReturnsNothing()
    {
        byte[] text = Encoding.ASCII.GetBytes("AAA");

        var mllpFramer = new MllpFramer();
        var result = mllpFramer.Push(text);

        Assert.Empty(result);
    }

    [Fact]
    public void Push_MessageSplitAcrossTwoPushes_ReturnsItAfterSecond()
    {
        var mllpFramer = new MllpFramer();
        var first = mllpFramer.Push(Encoding.ASCII.GetBytes("AA"));

        Assert.Empty(first);

        var second = mllpFramer.Push([.. Encoding.ASCII.GetBytes("A"), 0x1C, 0x0D]);

        Assert.Single(second);
        Assert.Equal(Encoding.ASCII.GetBytes("AAA"), second[0]);
    }

    [Fact]
    public void Push_EndMarkerSplit_ReturnsItAfterSecond()
    {
        var mllpFramer = new MllpFramer();
        var first = mllpFramer.Push([.. Encoding.ASCII.GetBytes("AAA"), 0x1C]);

        Assert.Empty(first);

        var second = mllpFramer.Push([0x0D]);

        Assert.Single(second);
        Assert.Equal(Encoding.ASCII.GetBytes("AAA"), second[0]);
    }

    [Fact]
    public void Push_TwoMessagesInOnePush_ReturnsBothInOrder()
    {
        var text1 = Encoding.ASCII.GetBytes("AAA");
        var text2 = Encoding.ASCII.GetBytes("BBB");
        byte[] input = [.. text1, 0x1C, 0x0D, .. text2, 0x1C, 0x0D];

        var mllpFramer = new MllpFramer();
        var result = mllpFramer.Push(input);

        Assert.Equal(2, result.Count);
        Assert.Equal([.. text1], result[0]);
        Assert.Equal([.. text2], result[1]);
    }

    [Fact]
    public void Push_LeadingStartByte_IsRemoved()
    {
        byte[] input = [0x0B, .. Encoding.ASCII.GetBytes("AAA"), 0x1C, 0x0D];

        var mllpFramer = new MllpFramer();
        var result = mllpFramer.Push(input);

        Assert.Single(result);
        Assert.Equal(Encoding.ASCII.GetBytes("AAA"), result[0]);
    }
}
