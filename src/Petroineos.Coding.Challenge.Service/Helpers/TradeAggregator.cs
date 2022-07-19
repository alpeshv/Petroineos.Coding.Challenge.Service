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
            var aggregatedTrade = PowerTrade.Create(DateTime.MinValue, 24);

            for (int y = 0; y < 24; y++)
            {
                double totalVolume = 0;
                foreach (var trade in trades)
                {
                    totalVolume += trade.Periods[y].Volume;
                }

                aggregatedTrade.Periods[y] = new PowerPeriod() { Period = y + 1, Volume = totalVolume };
            }

            var records = new List<PowerTradeVolume>();

            var time = new TimeOnly(23, 0);
            foreach (var period in aggregatedTrade.Periods)
            {
                records.Add(new PowerTradeVolume() { LocalTime = time, Volume = period.Volume });
                time = time.AddHours(1);
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
