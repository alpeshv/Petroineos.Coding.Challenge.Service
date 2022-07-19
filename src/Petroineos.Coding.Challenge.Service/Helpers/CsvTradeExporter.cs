using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Options;
using Petroineos.Coding.Challenge.Service.Interfaces;
using Petroineos.Coding.Challenge.Service.Models;

namespace Petroineos.Coding.Challenge.Service.Helpers
{
    public class CsvTradeExporter : ITradeExporter
    {
        private readonly ReportingOptions _reportingOptions;
        public CsvTradeExporter(IOptions<ReportingOptions> reportingOptions)
        {
            _reportingOptions = reportingOptions.Value;
        }
                
        public async Task ExportAsync(DateTime dateTime, IEnumerable<PowerTradeVolume> trades)
        {
            using var writer = new StreamWriter(Path.Join(_reportingOptions.ExportFolder, $"{GenerateFileName(dateTime)}.csv"));
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<PowerTradeVolume>();
            csv.NextRecord();

            await csv.WriteRecordsAsync(trades);
        }

        public static string GenerateFileName(DateTime dateTime)
        {
            return $"PowerPosition_{dateTime:yyyyMMdd_HHmm}";
        }
    }
}
