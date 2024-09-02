using System;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class MessageDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "ET";
    public static string[] DocumentTypeIdentifiers => ["MELDING"];

    [JsonPropertyName("Construction")]
    public Construction Construction { get; set; }
}
