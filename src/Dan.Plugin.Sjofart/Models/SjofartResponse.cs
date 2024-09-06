using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models;

[Serializable]
public class SjofartResponse<T>
{
    [JsonProperty(nameof(Offset))]
    public int Offset { get; set; }

    [JsonProperty(nameof(Count))]
    public int Count { get; set; }

    [JsonProperty(nameof(PageSize))]
    public int PageSize { get; set; }

    [JsonProperty(nameof(SearchResult))]
    public List<T> SearchResult { get; set; }
}
