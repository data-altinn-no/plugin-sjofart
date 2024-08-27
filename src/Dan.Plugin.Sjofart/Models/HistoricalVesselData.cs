using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Converters;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models;

// Model for response received from Sjøfartsdirektoratet. Not all fields are included in the model, only values
// that we actually need to use
public class HistoricalVesselData
{
    // Kjenningssignal
    [JsonPropertyName("CallSign")]
    public string CallSign { get; set; }

    // Norsk Ordinært Skipsregister - NOR
    // Norsk Internasjonalt Skipsregister - NIS
    // Skipsbyggingsregisteret - BYGG
    [JsonPropertyName("Register")]
    public string Register  { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Status")]
    public string Status { get; set; }

    [JsonPropertyName("IMO")]
    public long? Imo { get; set; }

    [JsonPropertyName("Documents")]
    [JsonProperty(ItemConverterType = typeof(VesselDocumentConverter))]
    public List<IVesselDocument> Documents { get; set; }
}
