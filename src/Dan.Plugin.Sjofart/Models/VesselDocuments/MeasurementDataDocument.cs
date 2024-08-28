using System;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class MeasurementDataDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "TE";
    public static string[] DocumentTypeIdentifiers => ["MÅLEBREV", "MÅLEDATA"];

    [JsonPropertyName("SrMeasurementData")]
    public SrMeasurementData SrMeasurementData { get; set; }
}
