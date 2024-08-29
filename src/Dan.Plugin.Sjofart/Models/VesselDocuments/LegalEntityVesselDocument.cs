using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public abstract class LegalEntityVesselDocument : VesselDocument
{
    [JsonPropertyName("Roles")]
    public List<DocumentRole> Roles { get; set; }

    public string GetLegalEntityIdForRoleType(string roleType)
    {
        return Roles
            .FirstOrDefault(r => string.Equals(r.RoleType, roleType, StringComparison.InvariantCultureIgnoreCase))?
            .LegalEntity
            .EntityId;
    }
}
