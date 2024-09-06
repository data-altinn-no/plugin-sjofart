using System;
using System.Collections.Generic;
using System.Linq;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

[Serializable]
public abstract class LegalEntityVesselDocument : VesselDocument
{
    [JsonProperty(nameof(Roles))]
    public List<DocumentRole> Roles { get; set; }

    public string GetLegalEntityIdForRoleType(string roleType)
    {
        return Roles
            .FirstOrDefault(r => string.Equals(r.RoleType, roleType, StringComparison.InvariantCultureIgnoreCase))?
            .LegalEntity
            .EntityId;
    }
}
