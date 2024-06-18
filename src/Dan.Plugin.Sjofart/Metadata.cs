using Dan.Common;
using Dan.Common.Enums;
using Dan.Common.Interfaces;
using Dan.Common.Models;
using Dan.Plugin.Sjofart.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Schema.Generation;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NJsonSchema;

namespace Dan.Plugin.Sjofart;

/// <summary>
/// All plugins must implement IEvidenceSourceMetadata, which describes that datasets returned by this plugin. An example is implemented below.
/// </summary>
public class Metadata : IEvidenceSourceMetadata
{

    private const string SOURCE = "Sjøfartsdirektoratet";
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<EvidenceCode> GetEvidenceCodes()
    {
        JSchemaGenerator generator = new JSchemaGenerator();

        return new List<EvidenceCode>()
        {
            new()
            {
                EvidenceCodeName = "Skipsregistrene",
                EvidenceSource = SOURCE,
                Values = new List<EvidenceValue>()
                {
                    new()
                    {
                        EvidenceValueName = "default",
                        ValueType = EvidenceValueType.JsonSchema,
                        JsonSchemaDefintion = JsonSchema.FromType<ResponseModel>().ToJson(Newtonsoft.Json.Formatting.Indented)
                    }
                }
            }
        };
    }


    /// <summary>
    /// This function must be defined in all DAN plugins, and is used by core to enumerate the available datasets across all plugins.
    /// Normally this should not be changed.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="context"></param>
    /// <returns></returns>
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
