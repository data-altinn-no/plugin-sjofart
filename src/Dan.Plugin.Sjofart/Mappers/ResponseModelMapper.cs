using System;
using System.Collections.Generic;
using System.Linq;
using Dan.Plugin.Sjofart.Config;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;

namespace Dan.Plugin.Sjofart.Mappers;

public class ResponseModelMapper : IMapper<HistoricalVesselData, ResponseModel>
{
    public ResponseModel Map(HistoricalVesselData input)
    {
        var responseModel = new ResponseModel
        {
            VesselId = input.VesselId,
            CallSign = input.CallSign,
            Registry = input.Register,
            Imo = input.Imo?.ToString(),
            Status = input.Status,
            ShipName = input.Name
        };

        var measurementDocument = GetNewestDocumentOfType<MeasurementDataDocument>(input.Documents);

        if (measurementDocument is not null)
        {
            responseModel.NetTonnage = measurementDocument.SrMeasurementData.NetTonnage;
            responseModel.GrossTonnage = measurementDocument.SrMeasurementData.GrossTonnage;
            responseModel.ShipType = measurementDocument.SrMeasurementData.VesselType;
        }

        // Find newest authority (hjemmel) document where there is a role named "eier"
        var authorityDocument = GetNewestDocumentOfType<AuthorityDocument>(
            input.Documents,
            d => d.Roles.Any(r => string.Equals(r.RoleType, PluginConstants.LegalEntityOwnerRole, StringComparison.InvariantCultureIgnoreCase)));

        // Finds newest deed (skjøte) document where there is a role named "eier"
        var deedDocument = GetNewestDocumentOfType<DeedDocument>(
            input.Documents,
            d => d.Roles.Any(r => string.Equals(r.RoleType, PluginConstants.LegalEntityOwnerRole, StringComparison.InvariantCultureIgnoreCase)));

        // Gets the newest of them and casts to LegalEntityVesselDocument to get the LegalEntity
        if (GetNewestOfDocuments(authorityDocument, deedDocument) is LegalEntityVesselDocument ownerDocument)
        {
            var owner = ownerDocument.Roles.First(r => string.Equals(r.RoleType, PluginConstants.LegalEntityOwnerRole, StringComparison.InvariantCultureIgnoreCase));
            responseModel.OwnerName = owner.LegalEntity.Name;
            responseModel.OwnerOrgNumber = owner.LegalEntity.EntityId;
        }

        // Liability information is found on SKJØTE documents
        if (deedDocument is not null)
        {
            responseModel.LiabilityAmount = deedDocument.Amount;
            responseModel.LiabilityCurrency = deedDocument.Currency;
            responseModel.LiabilityDate = deedDocument.Date;
        }

        // Find newest maintenance document where there is a role named "driftsselskap"
        var maintenanceDocument = GetNewestDocumentOfType<MaintenanceDocument>(
            input.Documents,
            d => d.Roles.Any(r => string.Equals(r.RoleType, PluginConstants.LegalEntityMaintenanceCompanyRole, StringComparison.InvariantCultureIgnoreCase)));

        if (maintenanceDocument is not null)
        {
            var mainentanceCompany = maintenanceDocument.Roles.First(r => string.Equals(r.RoleType, PluginConstants.LegalEntityMaintenanceCompanyRole, StringComparison.InvariantCultureIgnoreCase));
            responseModel.OperatingOrganisationName = mainentanceCompany.LegalEntity.Name;
            responseModel.OperationOrganisationNo = mainentanceCompany.LegalEntity.EntityId;
        }

        var messageDocument = GetNewestDocumentOfType<MessageDocument>(input.Documents);
        if (messageDocument is not null)
        {
            responseModel.YearBuilt = messageDocument.Construction?.Year;
        }

        var shipyardDocument = GetNewestDocumentOfType<ShipyardDocument>(input.Documents);
        if (shipyardDocument is not null)
        {
            responseModel.ShipYard = shipyardDocument.Construction?.Shipyard;
        }

        return responseModel;
    }

    private static T GetNewestDocumentOfType<T>(List<IVesselDocument> documents) where T : IVesselDocument
    {
        if (documents is null)
        {
            return default;
        }
        var documentsOfType = GetDocumentsOfType<T>(documents);
        return documentsOfType.FirstOrDefault();
    }

    private static T GetNewestDocumentOfType<T>(List<IVesselDocument> documents, Func<T, bool> predicate) where T : IVesselDocument
    {
        if (documents is null)
        {
            return default;
        }
        var documentsOfType = GetDocumentsOfType<T>(documents);
        return documentsOfType.FirstOrDefault(predicate);
    }

    private static IEnumerable<T> GetDocumentsOfType<T>(IEnumerable<IVesselDocument> documents) where T : IVesselDocument
    {
        return documents?
            .Where(d => d.GetType() == typeof(T))
            .Select(d => (T)d)
            .OrderByDescending(d => d.Date);
    }

    private static IVesselDocument GetNewestOfDocuments(params IVesselDocument[] documents)
    {
        if (documents is null || documents.Length == 0 || documents.All(d => d is null))
        {
            return default;
        }

        return documents
            .Where(d => d is not null)
            .OrderByDescending(d => d.Date).First();
    }
}
