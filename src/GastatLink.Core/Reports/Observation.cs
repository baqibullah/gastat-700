namespace GastatLink.Core.Reports;

public record Observation(string Code, string Name, decimal Value, string Unit, string ReferenceRange, string Flag);
