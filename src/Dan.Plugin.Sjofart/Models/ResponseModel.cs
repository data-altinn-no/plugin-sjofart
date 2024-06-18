using System;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Models;

[JsonObject]
public class ResponseModel
{
    [JsonProperty("eierOrgNr")]
    public string OwnerOrgNumber { get; set; }

    [JsonProperty("eierNavn")]
    public string OwnerName { get; set; }

    [JsonProperty("driftsselskapOrgNr")]
    public string OperationOrganisationNo { get; set; }

    [JsonProperty("driftsselskapNavn")]
    public string OperatingOrganisationName { get; set; }

    [JsonProperty("kjenningssignal")]
    public string CallSign { get; set; }

    [JsonProperty("skipsnavn")]
    public string ShipName { get; set; }

    [JsonProperty("register")]
    public string Registry { get; set; }

    [JsonProperty("imo-nummer")]
    public string IMO { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("byggeaar")]
    public int YearBuilt { get; set; }

    [JsonProperty("skipstype")]
    public string ShipType { get; set; }

    [JsonProperty("bruttotonnasje")]
    public double GrossTonnage { get; set; }

    [JsonProperty("nettotonnasje")]
    public double NetTonnage { get; set; }

    [JsonProperty("heftelsedato")]
    public DateTime LiabilityDate { get; set; }

    [JsonProperty("heftelsebeloep")]
    public double LiabilityAmount { get; set; }

    [JsonProperty("byggeverft")]
    public string ShipYard { get; set; }

}
