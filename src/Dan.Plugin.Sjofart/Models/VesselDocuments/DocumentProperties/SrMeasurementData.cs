using System;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

[Serializable]
public class SrMeasurementData
{
    [JsonPropertyName("VesselType")]
    public string VesselType { get; set; }

    [JsonPropertyName("GrossTonnage")]
    public double GrossTonnage { get; set; }

    [JsonPropertyName("NetTonnage")]
    public double NetTonnage { get; set; }
}
