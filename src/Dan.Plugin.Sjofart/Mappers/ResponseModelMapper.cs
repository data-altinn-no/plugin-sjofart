using System;
using System.Collections.Generic;
using System.Linq;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;

namespace Dan.Plugin.Sjofart.Mappers;

public class ResponseModelMapper : IMapper<HistoricalVesselData, ResponseModel>
{
    // TODO: Incomplete
    // TODO: const the strings used here, they are used elsewhere too
    public ResponseModel Map(HistoricalVesselData input)
    {
        var responseModel = new ResponseModel
        {
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
            d => d.Roles.Any(r => string.Equals(r.RoleType, "eier", StringComparison.InvariantCultureIgnoreCase)));

        if (ownerDocument is not null)
        {
            var owner = ownerDocument.Roles.First(r => string.Equals(r.RoleType, "eier", StringComparison.InvariantCultureIgnoreCase));
            responseModel.OwnerName = owner.LegalEntity.Name;
            responseModel.OwnerOrgNumber = owner.LegalEntity.EntityId;
        }

        var maintenanceDocument = GetNewestDocumentOfType<MaintenanceDocument>(
            input.Documents,
            d => d.Roles.Any(r => string.Equals(r.RoleType, "driftsselskap", StringComparison.InvariantCultureIgnoreCase)));

        if (maintenanceDocument is not null)
        {
            var mainentanceCompany = maintenanceDocument.Roles.First(r => string.Equals(r.RoleType, "driftsselskap", StringComparison.InvariantCultureIgnoreCase));
            responseModel.OperatingOrganisationName = mainentanceCompany.LegalEntity.Name;
            responseModel.OperationOrganisationNo = mainentanceCompany.LegalEntity.EntityId;
        }

        var messageDocument = GetNewestDocumentOfType<MessageDocument>(input.Documents);
        if (messageDocument is not null)
        {
            responseModel.YearBuilt = messageDocument.Construction.Year;
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
