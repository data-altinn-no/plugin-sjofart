using System;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dan.Plugin.Sjofart.Converters;

// SDir's documents in historical vessel data contains many types of documents with different values
// This converter is used to handle the deserialization of the documents we care about
public class VesselDocumentConverter : JsonConverter<IVesselDocument>
{
    public override void WriteJson(JsonWriter writer, IVesselDocument value, JsonSerializer serializer)
    {
        // We don't need to serialize back to SDir's data
        throw new NotImplementedException();
    }

    public override IVesselDocument ReadJson(JsonReader reader, Type objectType, IVesselDocument existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var variable = JObject.Load(reader);

        var documentType = variable.GetValue(IVesselDocument.DocumentTypeId)?.Value<string>();

        // TODO: look into a way to automate this better, just to cut down on potential human errors when adding new documents
        return documentType switch
        {
            MeasurementDataDocument.DocumentIdentifier => variable.ToObject<MeasurementDataDocument>(),
            MeasurementDataDocument.DocumentIdentifierAlternative => variable.ToObject<MeasurementDataDocument>(),
            AuthorityDocument.DocumentIdentifier => variable.ToObject<AuthorityDocument>(),
            AuthorityDocument.DocumentIdentifierAlternative => variable.ToObject<AuthorityDocument>(),
            MaintenanceDocument.DocumentIdentifier => variable.ToObject<MaintenanceDocument>(),
            MaintenanceDocument.DocumentIdentifierAlternative => variable.ToObject<MaintenanceDocument>(),
            MessageDocument.DocumentIdentifier => variable.ToObject<MessageDocument>(),
            ShipyardDocument.ShipyardDocumentIdentifier => variable.ToObject<ShipyardDocument>(),
            _ => variable.ToObject<UnusedDocument>()
        };
    }
}
