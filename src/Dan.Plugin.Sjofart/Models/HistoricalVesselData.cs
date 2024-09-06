using System;
using System.Collections.Generic;
using Dan.Plugin.Sjofart.Converters;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models;

// Model for response received from Sjøfartsdirektoratet. Not all fields are included in the model, only values
// that we actually need to use
[Serializable]
public class HistoricalVesselData
{
    [JsonProperty(nameof(VesselId))]
    public long? VesselId { get; set; }

    // Kjenningssignal
    [JsonProperty(nameof(CallSign))]
    public string CallSign { get; set; }

    // Norsk Ordinært Skipsregister - NOR
    // Norsk Internasjonalt Skipsregister - NIS
    // Skipsbyggingsregisteret - BYGG
    [JsonProperty(nameof(Register))]
    public string Register  { get; set; }

    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    [JsonProperty(nameof(Status))]
    public string Status { get; set; }

    [JsonProperty("IMO")]
    public long? Imo { get; set; }

    [JsonProperty(PropertyName = nameof(Documents), ItemConverterType = typeof(VesselDocumentConverter))]
    public List<IVesselDocument> Documents { get; set; }
}
