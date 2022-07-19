using Petroineos.Coding.Challenge.Service.Models;
using Services;

namespace Petroineos.Coding.Challenge.Service.Interfaces
{
    public interface ITradeAggregator
    {
        IEnumerable<PowerTradeVolume> Aggregate(IEnumerable<PowerTrade> trades);
    }
}