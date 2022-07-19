using Microsoft.Extensions.Options;
using Petroineos.Coding.Challenge.Service.Interfaces;
using Petroineos.Coding.Challenge.Service.Models;

namespace Petroineos.Coding.Challenge.Service
{
    public class Worker : BackgroundService
    {
        private readonly ReportingOptions _reportingOptions;
        private readonly IReportingService _reportingService;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IOptions<ReportingOptions> reportingOptions,
            IReportingService reportingService,
            ILogger<Worker> logger
            )
        {
            _reportingOptions = reportingOptions.Value;
            _reportingService = reportingService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var dateTime = DateTime.Now;

                    _logger.LogInformation("Start exporting trades for {DateTime}", dateTime);

                    await _reportingService.GenerateReportAsync(dateTime);

                    _logger.LogInformation("Finished exporting trades for {DateTime}", dateTime);

                    await Task.Delay(TimeSpan.FromMinutes(_reportingOptions.IntervalInMinutes), stoppingToken);                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                               
                Environment.Exit(1);
            }
        }
    }
}