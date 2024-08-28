using System;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class ShipyardDocument : VesselDocument
{
    public const string ShipyardDocumentIdentifier = "MELDING BYGGEVERFT";

    [JsonPropertyName("Construction")]
    public Construction Construction { get; set; }
}
