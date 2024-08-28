using System;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models;

// Documents from their API come in all shapes and forms, so just unite them under IVesselDocument
// to avoid creating any Dictionary<string, object> properties on HistoricalVesselData
public interface IVesselDocument
{
    const string DocumentTypeId = "DocumentType";
    const string DocumentTypeClassId = "DocumentTypeClass";
    const string DateId = "Date";

    string DocumentType { get; set; }

    string DocumentTypeClass { get; set; }
    DateTime Date { get; set; }
}

public abstract class VesselDocument : IVesselDocument
{
    [JsonPropertyName(IVesselDocument.DocumentTypeId)]
    public string DocumentType { get; set; }

    [JsonPropertyName(IVesselDocument.DocumentTypeClassId)]
    public string DocumentTypeClass { get; set; }

    [JsonPropertyName(IVesselDocument.DateId)]
    public DateTime Date { get; set; }
}
