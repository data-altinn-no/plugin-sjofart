using System;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class MessageDocument : IVesselDocument
{
    public const string DocumentIdentifier = "MELDING";

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("Construction")]
    public Construction Construction { get; set; }
}

public class Construction
{
    [JsonPropertyName("Year")]
    public int Year { get; set; }
}
