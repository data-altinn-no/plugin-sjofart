using System;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class MaintenanceDocument : LegalEntityVesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "HJ";
    public static string[] DocumentTypeIdentifiers => ["DRIFT IHT SKIPSSIKKERHETSLOVEN", "DRIFT IHHT SJØLOVEN/NIS LOVEN"];
}
