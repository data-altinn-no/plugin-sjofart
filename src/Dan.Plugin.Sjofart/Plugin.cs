using Dan.Common;
using Dan.Common.Exceptions;
using Dan.Common.Interfaces;
using Dan.Common.Models;
using Dan.Common.Util;
using Dan.Plugin.Sjofart.Config;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dan.Plugin.Sjofart.Clients;
using Dan.Plugin.Sjofart.Mappers;
using Dan.Plugin.Sjofart.Models;

namespace Dan.Plugin.Sjofart;

public class Plugin
{
    private readonly IEvidenceSourceMetadata _evidenceSourceMetadata;
    private readonly ILogger _logger;
    private readonly HttpClient _client;
    private readonly Settings _settings;
    private readonly IEntityRegistryService _erService;
    private readonly ISjofartClient _sjofartClient;
    private readonly IMapper<HistoricalVesselData, ResponseModel> _responseMapper;

    // The datasets must supply a human-readable source description from which they originate. Individual fields might come from different sources, and this string should reflect that (ie. name all possible sources).
    public const string SourceName = "Sjofartsdirektoratet";

    // These are not mandatory, but there should be a distinct error code (any integer) for all types of errors that can occur. The error codes does not have to be globally
    // unique. These should be used within either transient or permanent exceptions, see Plugin.cs for examples.
    private const int ERROR_UPSTREAM_UNAVAILBLE = 1001;
    private const int ERROR_INVALID_INPUT = 1002;
    private const int ERROR_NOT_FOUND = 1003;
    private const int ERROR_UNABLE_TO_PARSE_RESPONSE = 1004;

    public Plugin(
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory,
        IOptions<Settings> settings,
        IEvidenceSourceMetadata evidenceSourceMetadata,
        IEntityRegistryService entityRegistryService,
        ISjofartClient sjofartClient,
        IMapper<HistoricalVesselData, ResponseModel> responseMapper)
    {
        _client = httpClientFactory.CreateClient(Constants.SafeHttpClient);
        _logger = loggerFactory.CreateLogger<Plugin>();
        _settings = settings.Value;
        _evidenceSourceMetadata = evidenceSourceMetadata;
        _erService = entityRegistryService;
        _sjofartClient = sjofartClient;
        _responseMapper = responseMapper;

        _logger.LogDebug("Initialized plugin! This should be visible in the console");
    }

    [Function("Skipsregistrene")]
    public async Task<HttpResponseData> GetSkipsregistrene(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req,
        FunctionContext context)
    {

        var evidenceHarvesterRequest = await req.ReadFromJsonAsync<EvidenceHarvesterRequest>();

        return await EvidenceSourceResponse.CreateResponse(req,
            () => GetEvidenceValuesSimpledataset(evidenceHarvesterRequest));
    }

    private async Task<List<EvidenceValue>> GetEvidenceValuesSimpledataset(EvidenceHarvesterRequest evidenceHarvesterRequest)
    {
        var entity = await _erService.GetFull(evidenceHarvesterRequest.OrganizationNumber!, true, false, false);
        var vesselData = await _sjofartClient.GetVesselsByOrgNumber(evidenceHarvesterRequest.OrganizationNumber);
        var responseData = vesselData.Select(_responseMapper.Map).ToList();

        var ecb = new EvidenceBuilder(_evidenceSourceMetadata, "Skipsregistrene");
        ecb.AddEvidenceValue("default", responseData, SourceName);

        return ecb.GetEvidenceValues();
    }

    private async Task<T> MakeRequest<T>(string target)
    {
        HttpResponseMessage result;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, target);
            result = await _client.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new EvidenceSourceTransientException(ERROR_UPSTREAM_UNAVAILBLE, "Error communicating with upstream source", ex);
        }

        if (!result.IsSuccessStatusCode)
        {
            throw result.StatusCode switch
            {
                HttpStatusCode.NotFound => new EvidenceSourcePermanentClientException(ERROR_NOT_FOUND, "Upstream source could not find the requested entity (404)"),
                HttpStatusCode.BadRequest => new EvidenceSourcePermanentClientException(ERROR_INVALID_INPUT,  "Upstream source indicated an invalid request (400)"),
                _ => new EvidenceSourceTransientException(ERROR_UPSTREAM_UNAVAILBLE, $"Upstream source retuned an HTTP error code ({(int)result.StatusCode})")
            };
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to parse data returned from upstream source: {exceptionType}: {exceptionMessage}", ex.GetType().Name, ex.Message);
            throw new EvidenceSourcePermanentServerException(ERROR_UNABLE_TO_PARSE_RESPONSE, "Could not parse the data model returned from upstream source", ex);
        }
    }
}
