using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Dan.Common;
using Dan.Common.Exceptions;
using Dan.Plugin.Sjofart.Config;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Clients;

public interface ISjofartClient
{
    Task<IEnumerable<HistoricalVesselData>> GetVesselsByOrgNumber(string organiztionNumber);
}

public class SjofartClient(IHttpClientFactory clientFactory, IOptions<Settings> settings, ILoggerFactory loggerFactory) : ISjofartClient
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.SafeHttpClient);
    private readonly Settings _settings = settings.Value;
    private readonly ILogger<SjofartClient> _logger = loggerFactory.CreateLogger<SjofartClient>();

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
            var sjofartResponse = await MakeRequest<SjofartResponse<HistoricalVesselData>>(request);
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

    private async Task<T> MakeRequest<T>(HttpRequestMessage request)
    {
        HttpResponseMessage result;
        try
        {
            result = await _client.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new EvidenceSourceTransientException(PluginConstants.ErrorUpstreamUnavailble, "Error communicating with upstream source", ex);
        }

        if (!result.IsSuccessStatusCode)
        {
            throw result.StatusCode switch
            {
                HttpStatusCode.NotFound => new EvidenceSourcePermanentClientException(PluginConstants.ErrorNotFound, "Upstream source could not find the requested entity (404)"),
                HttpStatusCode.BadRequest => new EvidenceSourcePermanentClientException(PluginConstants.ErrorInvalidInput,  "Upstream source indicated an invalid request (400)"),
                _ => new EvidenceSourceTransientException(PluginConstants.ErrorUpstreamUnavailble, $"Upstream source retuned an HTTP error code ({(int)result.StatusCode})")
            };
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to parse data returned from upstream source: {exceptionType}: {exceptionMessage}", ex.GetType().Name, ex.Message);
            throw new EvidenceSourcePermanentServerException(PluginConstants.ErrorUnableToParseResponse, "Could not parse the data model returned from upstream source", ex);
        }
    }

    // Only interested in vessels that either are owned by or maintained by the organization being looked up
    public static List<HistoricalVesselData> GetRelevantVessels(IEnumerable<HistoricalVesselData> response, string organizationNumber)
    {
        return response
            .Where(sr =>
                sr.Documents.OfType<LegalEntityVesselDocument>().Any(d =>
                    d.GetLegalEntityIdForRoleType(PluginConstants.LegalEntityOwnerRole) == organizationNumber ||
                    d.GetLegalEntityIdForRoleType(PluginConstants.LegalEntityMaintenanceRole) == organizationNumber))
            .ToList();
    }
}
