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
}
