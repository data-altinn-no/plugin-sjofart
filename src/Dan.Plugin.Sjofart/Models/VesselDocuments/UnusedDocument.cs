using System;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

// Used to deserialize documents we do not use
public class UnusedDocument : IVesselDocument
{
    [JsonPropertyName("DocumentType")]
    public string DocumentType { get; set; }

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }
}
