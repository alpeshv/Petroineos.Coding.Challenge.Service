using Petroineos.Coding.Challenge.Service.Models;

namespace Petroineos.Coding.Challenge.Service.Interfaces
{
    public interface ITradeExporter
    {
        Task ExportAsync(DateTime dateTime, IEnumerable<PowerTradeVolume> trades);
    }
}