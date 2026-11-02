namespace GastatLink.Core.Reports;

public record GasReport
(
    string MessageControlId,
    DateTime ResultTime,
    string SpecimenId,
    string PatientId,
    string SampleType,
    IReadOnlyList<Observation> Observations
);
