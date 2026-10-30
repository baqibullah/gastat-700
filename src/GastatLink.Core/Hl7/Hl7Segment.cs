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
        // It will contain each segment defined like this:
        // ["OBX", "2", "NM", "3H080000002027052^pCO2^JC10", "", "28.2",...]
        _segmentParts.AddRange(line.Split("|"));
        // Item 0 is the segment type. eg -> MSH
        Name = _segmentParts[0];
        // MSH field defined like this:
        // MSH|^~\&|LD|IHE-J^LD|LIS|IHE-J^LIS|20120828114619||QBP^WOS^QBP_Q11|20120828114619234|P|2.5||||||~ISOIR87||ISO 2022-1994

        // After split by "|" will be like this
        //item 0: MSH
        // item 1: ^~\&
        // item 2: LD
        // item 3: IHE-J^LD
        // item 4: LIS
        // item 5: IHE-J^LIS
        // item 6: 20261029162054
        // item 7: (empty)
        // item 8: OUL^R22^OUL_R22
        // item 9: 20261029162054932

        // So we add "|" to match position of the Message Type and Message Control ID from the protocol definition.
        if (Name.Equals("MSH"))
        {
            _segmentParts.Insert(1, "|");
        }
    }

    public string Field(int number)
    {
        if (number < 0) throw new ArgumentOutOfRangeException(nameof(number), "Field number cannot be negative");
        return number < _segmentParts.Count ? _segmentParts[number] : "";
    }
}
