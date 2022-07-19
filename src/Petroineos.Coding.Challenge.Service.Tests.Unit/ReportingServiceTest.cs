using Petroineos.Coding.Challenge.Service.Interfaces;
using Petroineos.Coding.Challenge.Service.Models;
using Petroineos.Coding.Challenge.Service.Services;
using Services;

namespace Petroineos.Coding.Challenge.Service.Tests.Unit
{
    public class ReportingServiceTest
    {
        [Fact]
        public async Task GenerateReport_CoordinatesTradeExport()
        {
            var dateTime = DateTime.Now;

            Mock<IPowerService> powerServiceMock = new ();

            var trades = Array.Empty<PowerTrade>();
            powerServiceMock.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ReturnsAsync(trades);

            Mock<ITradeAggregator> tradeAggregatorMock = new();
            var trade = Array.Empty<PowerTradeVolume>();
            tradeAggregatorMock.Setup(x => x.Aggregate(It.IsAny<IEnumerable<PowerTrade>>())).Returns(trade);

            Mock<ITradeExporter> tradeExporterMock = new();

            var sut = new ReportingService(powerServiceMock.Object, tradeAggregatorMock.Object, tradeExporterMock.Object);
            await sut.GenerateReportAsync(dateTime);

            powerServiceMock.Verify(x=>x.GetTradesAsync(dateTime), Times.Once);
            tradeAggregatorMock.Verify(x=>x.Aggregate(trades), Times.Once);
            tradeExporterMock.Verify(x=>x.ExportAsync(dateTime, trade), Times.Once);
        }
    }
}