using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

// Only to be used on documents that have legal entity roles attached to the document
public abstract class LegalEntityVesselDocument : VesselDocument
{
    [JsonPropertyName("Roles")]
    public List<DocumentRole> Roles { get; set; }

    public string GetLegalEntityIdForRoleName(string roleName)
    {
        return Roles
            .FirstOrDefault(r => string.Equals(r.RoleType, roleName, StringComparison.InvariantCultureIgnoreCase))?
            .LegalEntity
            .EntityId;
    }
}
