namespace GastatLink.Core.Hl7;

public static class AckBuilder
{
    private const string MessageType = "OUL^R22^OUL_R22";

    /*
    Structure of ACK message:
    MSH|^~\&|{sendingApp}|{sendingFacility}|{receivingApp}|{receivingFacility}|{now:yyyyMMddHHmmss}||OUL^R22^OUL_R22|{now:yyyyMMddHHmmssfff}|P|2.5||||||~ISO IR87||ISO 2022-1994
    */
    public static string Build(Hl7Message incoming, DateTime now)
    {
        var msh = incoming.Segment("MSH") ?? throw new InvalidOperationException("Message has no MSH segment");
        var mshLine =
        $"MSH|^~\\&|{msh.Field(5)}|{msh.Field(6)}|{msh.Field(3)}|{msh.Field(4)}|{now:yyyyMMddHHmmss}||{MessageType}|{now:yyyyMMddHHmmssfff}|P|2.5||||||~ISO IR87||ISO 2022-1994";
        var msaLine = $"MSA|AA|{msh.Field(10)}";

        return mshLine + "\r" + msaLine + "\r";
    }
}
