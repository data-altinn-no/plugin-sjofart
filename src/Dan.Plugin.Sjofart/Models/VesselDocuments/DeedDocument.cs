using System;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class DeedDocument : LegalEntityVesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "HJ";

    public static string[] DocumentTypeIdentifiers => ["SKJØTE", "TVANGSSALGSSKJØTE"];

    [JsonProperty(nameof(Currency))]
    public string Currency  { get; set; }

    [JsonProperty(nameof(Amount))]
    public double? Amount { get; set; }
}
