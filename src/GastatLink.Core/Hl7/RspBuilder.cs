namespace GastatLink.Core.Hl7;

public static class RspBuilder
{
    private const string MessageType = "RSP^WOS^RSP_K11";
    private const string QueryStatus = "NF";
    private const string TestField = "3H080000002027000^ABG^JC10";

    /*
    Structure of Rsp message: defined in Hl7-protocol document

    MSH|^~\&|LIS|IHE-J^LIS|LD|IHE-J^LD|20120828114619||RSP^WOS^RSP_K11|20120828114619234|P|2.5||||||~ISOIR87||ISO 2022-1994<CR>
    MSA|AA|20090916120000<CR>
    QAK|20090916120000|OK|WOS^Work Order Step^IHE_LABTF<CR>
    QPD|WOS^Work Order Step^IHE_LABTF|20090916120000|^&&12345678901<CR>
    PID|||1234567890^^^^PI||^^^^^^L^I~John^Smith^^^^^L^P||19741115|M<CR>
    ORC|NW||||||||20090916120000<CR>
    OBR|1|||3H080000002027000^ABG^JC10<CR>
    OBX|1||3H080000002027000^ABG^JC10<CR>
    */
    public static string Build(Hl7Message qbp, DateTime now)
    {
        var msh = qbp.Segment("MSH") ?? throw new InvalidOperationException("Message has no MSH segment");
        var mshLine =
        $"MSH|^~\\&|{msh.Field(5)}|{msh.Field(6)}|{msh.Field(3)}|{msh.Field(4)}|{now:yyyyMMddHHmmss}||{MessageType}|{now:yyyyMMddHHmmssfff}|P|2.5||||||~ISO IR87||ISO 2022-1994";
        var msaLine = $"MSA|AA|{msh.Field(10)}";

        var qpd = qbp.Segment("QPD") ?? throw new InvalidOperationException("Message has no QPD segment");
        var qakLine = $"QAK|{qpd.Field(2)}|{QueryStatus}|{qpd.Field(1)}";
        var qpdLine = $"QPD|{qpd.Field(1)}|{qpd.Field(2)}|{qpd.Field(3)}";
        var pidLine = "PID||||||||";
        var orcLine = $"ORC|NW||||||||{now:yyyyMMddHHmmss}";
        var obrLine = $"OBR|1|||{TestField}";
        var obxLine = $"OBX|1||{TestField}";

        return mshLine + "\r" + msaLine + "\r" + qakLine + "\r" + qpdLine + "\r" + pidLine + "\r" + orcLine + "\r" + obrLine + "\r" + obxLine + "\r";
    }
}
