using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

public class Construction
{
    [JsonPropertyName("Year")]
    public int? Year { get; set; }

    [JsonPropertyName("Shipyard")]
    public string Shipyard { get; set; }
}
