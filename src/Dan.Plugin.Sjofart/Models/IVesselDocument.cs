using System;

namespace Dan.Plugin.Sjofart.Models;

// Documents from their API come in all shapes and forms, so just unite them under IVesselDocument
// to avoid creating any Dictionary<string, object> properties on HistoricalVesselData
public interface IVesselDocument
{
    const string DocumentType = "DocumentType";
    DateTime Date { get; }
}
