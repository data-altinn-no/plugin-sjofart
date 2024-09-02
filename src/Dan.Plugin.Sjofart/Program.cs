using Dan.Plugin.Sjofart;
using Microsoft.Extensions.Hosting;
using Dan.Common.Extensions;
using Dan.Plugin.Sjofart.Clients;
using Dan.Plugin.Sjofart.Config;
using Dan.Plugin.Sjofart.Mappers;
using Dan.Plugin.Sjofart.Models;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureDanPluginDefaults()
    .ConfigureAppConfiguration((context, configuration) =>
    {
        // Add more configuration sources if necessary. ConfigureDanPluginDefaults will load environment variables, which includes
        // local.settings.json (if developing locally) and applications settings for the Azure Function
    })
    .ConfigureServices((context, services) =>
    {
        // Add any additional services here
        services.AddTransient<ISjofartClient, SjofartClient>();
        services.AddTransient<IMapper<HistoricalVesselData, ResponseModel>, ResponseModelMapper>();

        // This makes IOption<Settings> available in the DI container.
        var configurationRoot = context.Configuration;
        services.Configure<Settings>(configurationRoot);
    })
    .Build();

await host.RunAsync();
