namespace GastatLink.Core.Hl7;


// Cuts the stream of bytes into whole HL7 messages. Since TCP sends a stream of bytes.
// Standard message look like this:
// 0B <message text> 1C OD
public sealed class MllpFramer
{
    // (11 in decimal) is the vertical tab character |
    private const byte StartByte = 0x0B;
    // (28 in decimal) is the file separator character
    private const byte EndByte1 = 0x1C;
    // (13 in decimal) CR which is carriage return
    private const byte EndByte2 = 0x0D;

    private readonly List<byte> _buffer = [];

    // A message can arrive in stream of bytes and TCP can chop message into pieces.
    // _buffer is a bucket we use to keep track of everything that has arrived not handed yet.
    // Lets assume message arrive in this format:
    // slice 1: AA
    // slice 2: A<END>BB
    // slice 3: B<END>
    // AAA is message 1 BBB is message 2 and <END> is 1C 0D
    public IReadOnlyList<byte[]> Push(ReadOnlySpan<byte> data)
    {
        _buffer.AddRange(data);
        var messages = new List<byte[]>();

        while (true)
        {
            int end = IndexOfEnd();
            // We wait for new message since it has no <END>
            if (end < 0) break;

            // We copy the message from 0 - <END>
            var message = _buffer.GetRange(0, end);
            _buffer.RemoveRange(0, end + 2);

            if (message.Count > 0 && message[0] == StartByte)
                message.RemoveAt(0);

            messages.Add([.. message]);
        }
        return messages;
    }

    private int IndexOfEnd()
    {
        for (int i = 0; i < _buffer.Count - 1; i++)
        {
            if (_buffer[i] == EndByte1 && _buffer[i + 1] == EndByte2)
            {
                return i;
            }
        }
        return -1;
    }
}
