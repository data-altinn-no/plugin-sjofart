using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class AuthorityDocument : LegalEntityVesselDocument
{
    public const string DocumentIdentifier = "HJEMMELSDOKUMENT";
    public const string DocumentIdentifierAlternative = "SKJØTE";
}
