using System;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public class AuthorityDocument : LegalEntityVesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "HJ";
    public static string[] DocumentTypeIdentifiers => ["HJEMMELSDOKUMENT", "BYGGEBREV"];
}
