using Dan.Common.Exceptions;
using Dan.Common.Interfaces;
using Dan.Common.Models;
using Dan.Common.Util;
using Dan.Plugin.Sjofart.Config;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dan.Plugin.Sjofart.Clients;
using Dan.Plugin.Sjofart.Mappers;
using Dan.Plugin.Sjofart.Models;

namespace Dan.Plugin.Sjofart;

public class Plugin
{
    private readonly IEvidenceSourceMetadata _evidenceSourceMetadata;
    private readonly ILogger _logger;
    private readonly IEntityRegistryService _erService;
    private readonly ISjofartClient _sjofartClient;
    private readonly IMapper<HistoricalVesselData, ResponseModel> _responseMapper;

    public Plugin(
        ILoggerFactory loggerFactory,
        IEvidenceSourceMetadata evidenceSourceMetadata,
        IEntityRegistryService entityRegistryService,
        ISjofartClient sjofartClient,
        IMapper<HistoricalVesselData, ResponseModel> responseMapper)
    {
        _logger = loggerFactory.CreateLogger<Plugin>();
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
        EvidenceHarvesterRequest evidenceHarvesterRequest;
        try
        {
            evidenceHarvesterRequest = await req.ReadFromJsonAsync<EvidenceHarvesterRequest>();
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Exception while attempting to parse request into EvidenceHarvesterRequest: {exceptionType}: {exceptionMessage}",
                e.GetType().Name, e.Message);
            throw new EvidenceSourcePermanentClientException(PluginConstants.ErrorInvalidInput,
                "Unable to parse request", e);
        }

        return await EvidenceSourceResponse.CreateResponse(req,
            () => GetEvidenceValuesSimpledataset(evidenceHarvesterRequest));
    }

    private async Task<List<EvidenceValue>> GetEvidenceValuesSimpledataset(EvidenceHarvesterRequest evidenceHarvesterRequest)
    {
        if (evidenceHarvesterRequest?.OrganizationNumber is null)
        {
            throw new EvidenceSourcePermanentClientException(PluginConstants.ErrorInvalidInput,
                "Request is missing organization number");
        }

        var entity = await _erService.GetFull(evidenceHarvesterRequest.OrganizationNumber);
        if (entity is null)
        {
            throw new EvidenceSourcePermanentClientException(PluginConstants.ErrorNotFound, $"Legal entity ({evidenceHarvesterRequest.OrganizationNumber})not found");
        }

        var vesselData = await _sjofartClient.GetVesselsByOrgNumber(evidenceHarvesterRequest.OrganizationNumber);
        List<ResponseModel> responseData;
        try
        {
            responseData = vesselData.Select(_responseMapper.Map).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to map SDir's response into ResponesModel: {exceptionType}: {exceptionMessage}", e.GetType().Name, e.Message);
            throw new EvidenceSourcePermanentServerException(PluginConstants.ErrorUnableToParseResponse, "Unable to map upstream response into response model", e);
        }

        var ecb = new EvidenceBuilder(_evidenceSourceMetadata, "Skipsregistrene");
        ecb.AddEvidenceValue("default", responseData, PluginConstants.EvidenceSource);

        return ecb.GetEvidenceValues();
    }
}
