﻿using System;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class MeasurementDataDocument : IVesselDocument
{
    // TODO: Maybe we can use an array?
    public const string DocumentIdentifier = "MÅLEBREV";
    public const string DocumentIdentifierAlternative = "MÅLEDATA";

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("SrMeasurementData")]
    public SrMeasurementData SrMeasurementData { get; set; }
}

public class SrMeasurementData
{
    [JsonPropertyName("VesselType")]
    public string VesselType { get; set; }

    [JsonPropertyName("GrossTonnage")]
    public double GrossTonnage { get; set; }

    [JsonPropertyName("NetTonnage")]
    public double NetTonnage { get; set; }
}
