using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Threading.Tasks;
using System.Web;
using Dan.Common;
using Dan.Plugin.Sjofart.Config;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Clients;

public interface ISjofartClient
{
    Task<IEnumerable<HistoricalVesselData>> GetVesselsByOrgNumber(string organiztionNumber);
}

public class SjofartClient(IHttpClientFactory clientFactory, IOptions<Settings> settings) : ISjofartClient
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.SafeHttpClient);
    private readonly Settings _settings = settings.Value;

    private const int MaxPageSize = 100;

    public async Task<IEnumerable<HistoricalVesselData>> GetVesselsByOrgNumber(string organiztionNumber)
    {
        var baseAddress = _settings.EndpointUrl;
        const string path = "/vessel-search/search/historicalvesseldata";

        var currentOffset = 0;
        int totalCount;
        var vesselData = new List<HistoricalVesselData>();
        do
        {
            var request = GetRequest($"{baseAddress}{path}", organiztionNumber, currentOffset);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var sjofartResponse = JsonConvert.DeserializeObject<SjofartResponse<HistoricalVesselData>>(content);

            vesselData.AddRange(sjofartResponse.SearchResult);

            totalCount = sjofartResponse.Count;
            currentOffset += sjofartResponse.PageSize;
        } while (vesselData.Count < totalCount);

        return GetRelevantVessels(vesselData, organiztionNumber);
    }

    private HttpRequestMessage GetRequest(string url, string organiztionNumber, int currentOffset)
    {
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["LegalEntityId"] = organiztionNumber;
        query["MaxPerPage"] = MaxPageSize.ToString();
        query["Offset"] = currentOffset.ToString();
        uriBuilder.Query = query.ToString()!;
        var uri = uriBuilder.ToString();
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {_settings.ApiToken}");

        return request;
    }

    // Only interested in vessels that either are owned by or maintained by the organization being looked up
    private static List<HistoricalVesselData> GetRelevantVessels(IEnumerable<HistoricalVesselData> response, string organizationNumber)
    {
        // TODO: const the strings used here, they are used elsewhere too
        return response
            .Where(sr =>
                sr.Documents.OfType<ILegalEntityVesselDocument>().Any(d =>
                    d.GetLegalEntityIdForRoleName("eier") == organizationNumber ||
                    d.GetLegalEntityIdForRoleName("driftsansvarlig") == organizationNumber))
            .ToList();
    }
}
