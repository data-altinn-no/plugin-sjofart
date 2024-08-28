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

        var ownerDocument = GetNewestDocumentOfType<AuthorityDocument>(
            input.Documents,
            d => d.Roles.Any(r => string.Equals(r.RoleType, PluginConstants.LegalEntityOwnerRole, StringComparison.InvariantCultureIgnoreCase)));

        if (ownerDocument is not null)
        {
            var owner = ownerDocument.Roles.First(r => string.Equals(r.RoleType, PluginConstants.LegalEntityOwnerRole, StringComparison.InvariantCultureIgnoreCase));
            responseModel.OwnerName = owner.LegalEntity.Name;
            responseModel.OwnerOrgNumber = owner.LegalEntity.EntityId;
        }

        var maintenanceDocument = GetNewestDocumentOfType<MaintenanceDocument>(
            input.Documents,
            d => d.Roles.Any(r => string.Equals(r.RoleType, PluginConstants.LegalEntityMaintenanceCompanyRole, StringComparison.InvariantCultureIgnoreCase)));

        // TODO: Doesn't seem to be reliable enough
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

        // TODO: We need to know how to get liability data

        return responseModel;
    }

    private static T GetNewestDocumentOfType<T>(List<IVesselDocument> documents) where T : IVesselDocument
    {
        var documentsOfType = GetDocumentsOfType<T>(documents);
        return documentsOfType.FirstOrDefault();
    }

    private static T GetNewestDocumentOfType<T>(List<IVesselDocument> documents, Func<T, bool> predicate) where T : IVesselDocument
    {
        var documentsOfType = GetDocumentsOfType<T>(documents);
        return documentsOfType.FirstOrDefault(predicate);
    }

    private static IEnumerable<T> GetDocumentsOfType<T>(IEnumerable<IVesselDocument> documents) where T : IVesselDocument
    {
        return documents
            .Where(d => d.GetType() == typeof(T))
            .Select(d => (T)d)
            .OrderByDescending(d => d.Date);
    }
}
