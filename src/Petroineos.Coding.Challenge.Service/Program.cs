using Petroineos.Coding.Challenge.Service;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Services;
using Petroineos.Coding.Challenge.Service.Interfaces;
using Petroineos.Coding.Challenge.Service.Services;
using Petroineos.Coding.Challenge.Service.Models;
using Petroineos.Coding.Challenge.Service.Helpers;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Reporting Service";
    })    
    .ConfigureServices((context, services) =>
    {
        services.Configure<ReportingOptions>(
            context.Configuration.GetSection(
                nameof(ReportingOptions)));

        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);

        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<ITradeAggregator, TradeAggregator>();
        services.AddSingleton<ITradeExporter, CsvTradeExporter>();
        services.AddSingleton<IReportingService, ReportingService>();
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging((context, logging) =>
    {        
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();

await host.RunAsync();