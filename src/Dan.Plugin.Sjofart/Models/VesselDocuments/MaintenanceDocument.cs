using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class MaintenanceDocument : ILegalEntityVesselDocument
{
    public const string DocumentIdentifier = "DRIFT IHT SKIPSSIKKERHETSLOVEN";
    public const string DocumentIdentifierAlternative = "DRIFT IHHT SJØLOVEN/NIS LOVEN";

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("Roles")]
    public List<DocumentRole> Roles { get; set; }
}
