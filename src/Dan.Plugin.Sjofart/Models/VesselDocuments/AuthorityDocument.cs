using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class AuthorityDocument : ILegalEntityVesselDocument
{
    public const string DocumentIdentifier = "HJEMMELSDOKUMENT";
    public const string DocumentIdentifierAlternative = "SKJØTE";

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("roles")]
    public List<DocumentRole> Roles { get; set; }
}
