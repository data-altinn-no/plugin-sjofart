using System;
using System.Linq;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dan.Plugin.Sjofart.Converters;

// SDir's documents in historical vessel data contains many objects of different structures
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
        var document = JObject.Load(reader);

        // Find the document identifiers
        var documentType = document.GetValue(IVesselDocument.DocumentTypeId)?.Value<string>();
        var documentTypeClass = document.GetValue(IVesselDocument.DocumentTypeClassId)?.Value<string>();

        // Get all implementations of IVesselDocument that is concrete,
        // implements IDocumentIdentifiable and is not UnusedDocument
        var vesselDocumentTypes = typeof(IVesselDocument)
            .Assembly
            .GetTypes()
            .Where(t =>
                typeof(IDocumentIdentifiable).IsAssignableFrom(t) &&
                typeof(IVesselDocument).IsAssignableFrom(t) &&
                t.IsClass &&
                !t.IsAbstract &&
                t != typeof(UnusedDocument))
            .ToList();

        // Find the document type that matches the current's document class and document class type
        var vesselDocumentType = vesselDocumentTypes
            .FirstOrDefault(t => (string)t.GetProperty("DocumentTypeClassIdentifier")?.GetValue(null) == documentTypeClass &&
                                 ((string[])t.GetProperty("DocumentTypeIdentifiers")?.GetValue(null) ?? []).Contains(documentType));

        // If no match, deserialize to unused document
        if (vesselDocumentType == null)
        {
            return document.ToObject<UnusedDocument>();
        }

        // Match found, deserialize object and pass it back as IVesselDocument
        return document.ToObject(vesselDocumentType) as IVesselDocument;
    }
}
