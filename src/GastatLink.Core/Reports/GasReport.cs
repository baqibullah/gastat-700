namespace GastatLink.Core.Reports;

public record GasReport
(
    string MessageControlId,
    DateTime ResultTime,
    string SpecimenId,
    string PatientId,
    string PatientName,
    DateOnly? DateOfBirth,
    string Sex,
    string SampleType,
    IReadOnlyList<Observation> Observations
);
