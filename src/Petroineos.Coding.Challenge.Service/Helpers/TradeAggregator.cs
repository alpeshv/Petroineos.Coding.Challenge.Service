using System.Collections.Concurrent;
using Petroineos.Coding.Challenge.Service.Interfaces;
using Petroineos.Coding.Challenge.Service.Models;
using Services;

namespace Petroineos.Coding.Challenge.Service.Helpers
{
    public class TradeAggregator : ITradeAggregator
    {        
        public IEnumerable<PowerTradeVolume> Aggregate(IEnumerable<PowerTrade> trades)
        {
            double[] volumes = new double[24];

            foreach (var trade in trades)
            {
                foreach (var period in trade.Periods)
                {
                    volumes[period.Period-1] += period.Volume;
                }
            };

            var records = new List<PowerTradeVolume>();

            var time = new TimeOnly(23, 0);

            var index = 0;
            foreach (var volume in volumes)
            {
                records.Add(new PowerTradeVolume() { LocalTime = time.AddHours(index), Volume = volume });
                index++;
            }

            return records;
        }

        [Obsolete]
        public IEnumerable<PowerTradeVolume> AggregateParallel(IEnumerable<PowerTrade> trades)
        {
            int initialCapacity = 24;

            int numProcs = Environment.ProcessorCount;
            int concurrencyLevel = numProcs * 2;

            ConcurrentDictionary<int, double> cd = new(concurrencyLevel, initialCapacity);

            Parallel.ForEach(trades, trade =>
            {
                Parallel.ForEach(trade.Periods, period =>
                    cd.AddOrUpdate(period.Period, period.Volume, (key, value) => { return value + period.Volume; }));
            });

            var records = new List<PowerTradeVolume>();

            var time = new TimeOnly(23, 0);

            foreach (var item in cd.OrderBy(x => x.Key))
            {
                records.Add(new PowerTradeVolume() { LocalTime = time.AddHours(item.Key - 1), Volume = item.Value });
            }

            return records;
        }
    }
}
