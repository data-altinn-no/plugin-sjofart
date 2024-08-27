using System;
using System.Collections.Generic;
using System.Linq;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

// Only to be used on documents that have legal entity roles attached to the document
public interface ILegalEntityVesselDocument : IVesselDocument
{
    public List<DocumentRole> Roles { get; }

    // TODO: This is duped across classes, figure out a way to avoid that. Shared abstract class maybe?
    public string GetLegalEntityIdForRoleName(string roleName)
    {
        return Roles
            .FirstOrDefault(r => string.Equals(r.RoleType, roleName, StringComparison.InvariantCultureIgnoreCase))?
            .LegalEntity
            .EntityId;
    }
}
