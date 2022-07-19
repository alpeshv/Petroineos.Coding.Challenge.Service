using FluentAssertions;
using Petroineos.Coding.Challenge.Service.Helpers;

namespace Petroineos.Coding.Challenge.Service.Tests.Unit.Services
{
    public class CsvTradeExporterTests
    {
        [Fact]
        public void GenerateFileName_GeneratesFileNameFromProvidedDate()
        {                                    
            var dateTime = new DateTime(2022, 12, 28, 18, 37, 55);
            var fileName = CsvTradeExporter.GenerateFileName(dateTime);

            fileName.Should().Be("PowerPosition_20221228_1837");
        }
    }
}
