using System;
using System.Text.Json.Serialization;
using Google.Protobuf.WellKnownTypes;

namespace Dan.Plugin.Sjofart.Models;

// Documents from their API come in all shapes and forms, so just unite them under IVesselDocument
// to avoid creating any Dictionary<string, object> properties on HistoricalVesselData
public interface IVesselDocument
{
    const string DocumentTypeId = "DocumentType";
    const string DocumentTypeClassId = "DocumentTypeClass";
    const string DateId = "Date";

    DateTime Date { get; set; }
}

// Used to identify what class the elements in the Documents array from SDir should be deserialised into
// For example, to get NetTonnage and GrossTonnage, the document that information is on is
// DocumentTypeClass: TE, but it can be DocumentType: MÅLEBREV or MÅLEDATA, meaning we don't have a single identifier
// for the information we need. The combination of DocumentTypeClass with a matching DocumentType is our best effort
public interface IDocumentIdentifiable
{
    public static abstract string DocumentTypeClassIdentifier { get; }
    public static abstract string[] DocumentTypeIdentifiers { get; }
}

public abstract class VesselDocument : IVesselDocument
{
    [JsonPropertyName(IVesselDocument.DateId)]
    public DateTime Date { get; set; }
}
