namespace Dan.Plugin.Sjofart.Config;

public class Settings
{
    public int DefaultCircuitBreakerOpenCircuitTimeSeconds { get; init; }
    public int DefaultCircuitBreakerFailureBeforeTripping { get; init; }
    public int SafeHttpClientTimeout { get; init; }

    public string EndpointUrl { get; init; }
    public string ApiToken { get; init; }
}
