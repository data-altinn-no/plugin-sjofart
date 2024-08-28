using System;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

[Serializable]
public class DocumentRole
{
    [JsonPropertyName("LegalEntity")]
    public LegalEntity LegalEntity { get; set; }

    [JsonPropertyName("RoleType")]
    public string RoleType { get; set; }
}

[Serializable]
public class LegalEntity
{
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("EntityId")]
    public string EntityId { get; set; }
}
