using System;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class ShipyardDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "ET";
    public static string[] DocumentTypeIdentifiers => ["MELDING BYGGEVERFT"];

    [JsonPropertyName("Construction")]
    public Construction Construction { get; set; }
}
