using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class MaintenanceDocument : LegalEntityVesselDocument
{
    public const string DocumentIdentifier = "DRIFT IHT SKIPSSIKKERHETSLOVEN";
    public const string DocumentIdentifierAlternative = "DRIFT IHHT SJØLOVEN/NIS LOVEN";
}
