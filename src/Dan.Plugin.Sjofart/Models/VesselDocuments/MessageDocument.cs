using System;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class MessageDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "ET";
    public static string[] DocumentTypeIdentifiers => ["MELDING"];

    [JsonProperty(nameof(Construction))]
    public Construction Construction { get; set; }
}
