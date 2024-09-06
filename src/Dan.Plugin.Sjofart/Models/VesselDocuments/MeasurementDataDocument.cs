using System;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class MeasurementDataDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "TE";

    // Allegedly net and gross tonnage can't be fetched from måledata, only målebrev, but I've found examples of
    // måledata that has tonnage, so I'm just gonna keep it. It'll still be null if it can't be found
    public static string[] DocumentTypeIdentifiers => ["MÅLEBREV", "MÅLEDATA"];

    [JsonProperty(nameof(SrMeasurementData))]
    public SrMeasurementData SrMeasurementData { get; set; }
}
