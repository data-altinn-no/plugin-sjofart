﻿using System;
using System.Text.Json.Serialization;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Models.VesselDocuments;

public class MessageDocument : VesselDocument
{
    public const string DocumentIdentifier = "MELDING";

    [JsonPropertyName("Construction")]
    public Construction Construction { get; set; }
}
