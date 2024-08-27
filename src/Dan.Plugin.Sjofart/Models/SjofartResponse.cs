using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dan.Plugin.Sjofart.Models;

public class SjofartResponse<T>
{
    [JsonPropertyName("Offset")]
    public int Offset { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("PageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("SearchResult")]
    public List<T> SearchResult { get; set; }
}
