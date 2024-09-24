using Dan.Common;
using Dan.Common.Enums;
using Dan.Common.Interfaces;
using Dan.Common.Models;
using Dan.Plugin.Sjofart.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Dan.Plugin.Sjofart.Config;
using NJsonSchema;

namespace Dan.Plugin.Sjofart;

public class Metadata : IEvidenceSourceMetadata
{
    const string EDueDiligence = "eDueDiligence";
    private readonly List<string> _belongsToEDueDiligence = [EDueDiligence];

    public List<EvidenceCode> GetEvidenceCodes()
    {
        return
        [
            new EvidenceCode
            {
                EvidenceCodeName = "Skipsregistrene",
                EvidenceSource = PluginConstants.EvidenceSource,
                ServiceContext = EDueDiligence,
                BelongsToServiceContexts = _belongsToEDueDiligence,
                Values =
                [
                    new EvidenceValue
                    {
                        EvidenceValueName = "default",
                        ValueType = EvidenceValueType.JsonSchema,
                        JsonSchemaDefintion = JsonSchema
                            .FromType<ResponseModel>()
                            .ToJson(Newtonsoft.Json.Formatting.Indented)
                    }
                ]
            }
        ];
    }

    [Function(Constants.EvidenceSourceMetadataFunctionName)]
    public async Task<HttpResponseData> GetMetadataAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req,
        FunctionContext context)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(GetEvidenceCodes());
        return response;
    }

}
