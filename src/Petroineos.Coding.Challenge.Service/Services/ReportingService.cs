using Petroineos.Coding.Challenge.Service.Interfaces;
using Polly;
using Services;

namespace Petroineos.Coding.Challenge.Service.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IPowerService _powerService;
        private readonly ITradeAggregator _tradeAggregator;
        private readonly ITradeExporter _tradeExporter;

        public ReportingService(IPowerService powerService, ITradeAggregator tradeAggregator, ITradeExporter tradeExporter)
        {
            _powerService = powerService;
            _tradeAggregator = tradeAggregator;
            _tradeExporter = tradeExporter;
        }

        public async Task GenerateReportAsync(DateTime dateTime)
        {
            var retryPolicy = Policy.Handle<PowerServiceException>()
                                .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: _ => TimeSpan.FromSeconds(1));

            var trades = await retryPolicy.ExecuteAsync(async () =>
            {
                 return await _powerService.GetTradesAsync(dateTime);
            });
            
            var aggregatedTrade = _tradeAggregator.Aggregate(trades);
            await _tradeExporter.ExportAsync(dateTime, aggregatedTrade);
        }
    }
}
