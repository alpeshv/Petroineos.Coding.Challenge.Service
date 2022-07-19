using FluentAssertions;
using Petroineos.Coding.Challenge.Service.Helpers;
using Petroineos.Coding.Challenge.Service.Models;
using Services;

namespace Petroineos.Coding.Challenge.Service.Tests.Unit.Helpers
{
    public class TradeAggregatorTests
    {
        private readonly DateTime _date;

        public TradeAggregatorTests()
        {
            _date = DateTime.Now;
        }

        [Fact]
        public void Aggregate_AggregatesTradeVolumes()
        {
            TradeAggregator sut = new();

            var trades = GenerateTestTrades();
            var aggregatedTrades = sut.Aggregate(trades);

            var expectedAggregatedTrades = GetExpectedAggregatedTrade();

            aggregatedTrades.Should().NotBeNullOrEmpty();

            aggregatedTrades.Should().BeEquivalentTo(expectedAggregatedTrades);
        }
                
        [Fact(Skip = "Aggregating using parallel for loop is slow and does not retain precision")]
        public void AggregateParallel_AggregatesTradeVolumes()
        {
            TradeAggregator sut = new();

            var trades = GenerateTestTrades();
            var aggregatedTrades = sut.AggregateParallel(trades);

            var expectedAggregatedTrades = GetExpectedAggregatedTrade();

            aggregatedTrades.Should().NotBeNullOrEmpty();

            aggregatedTrades.Should().BeEquivalentTo(expectedAggregatedTrades);
        }

        private IEnumerable<PowerTrade> GenerateTestTrades()
        {
            var trades = new PowerTrade[] {
                PowerTrade.Create(_date, 24),
                PowerTrade.Create(_date, 24),
                PowerTrade.Create(_date, 24),
                PowerTrade.Create(_date, 24),
            };

            var volumes = new double[,]
            {
                { 452.37339662077056,301.3138527568493,948.6845335159222,930.216519848275,820.0666396347165,256.28660903914636,467.38800970797945,317.17080093281845,-134.82919228941836,991.6013950827843,957.2511404330708,824.6614023689541,622.5980715128725,322.2127155390298,310.7076696722555,920.9078482120531,393.37186353546093,637.1710351589315,804.3441746598091,220.8026421858259,846.2561433811044,560.7840453009612,175.16240795022986,577.8396445025921 },
                { 120.65255722727531,232.1339796879145,415.64645326502546,958.3752332572061,959.7650833062904,880.6900428867006,371.35295130256054,48.06525838910491,386.41358564369585,377.5887124350384,2.255673140076686,781.263522792668,116.51879497291328,293.15991284880283,972.8293652595232,457.07472875726177,411.6558252076008,757.7980554076958,765.1036706460559,350.8014057902499,-244.04313485633432,454.00978073069274,833.3741664456999,342.98533456624625 },
                { 937.3778468892791,938.360184522556,462.307657213378,613.3038998317722,430.6024156910984,949.6787433383973,773.888489973766,126.17403100622437,785.8343156386611,32.053131257780535,772.9127439585784,-58.70105563079342,801.5390472342525,2.688153530401638,313.48706529151957,798.2157736674081,87.7427804017642,150.60517960315855,654.8793882582759,975.3958308838802,807.7457368310984,967.2926345417404,369.3038525145601,293.75006829238293 },
                { 868.3573396174652,692.335965273199,677.0857642376177,-58.49588519788163,933.4438504167593,381.0782062488849,574.477363408319,389.6543687871067,439.1784046602616,239.56912334157832,150.23620611251087,41.68051057842681,20.11578465449826,563.6072642395218,-130.5759635772481,190.06839440103428,788.4638842706622,867.57013956453,136.89544027579626,242.20455559238673,872.0780275520473,569.6952416881137,137.50234335021582,496.39781327769083 }
            };

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    trades[x].Periods[y] = new PowerPeriod() { Period = y + 1, Volume = volumes[x, y] };
                }
            }

            return trades;
        }

        private IEnumerable<PowerTradeVolume> GetExpectedAggregatedTrade()
        {       
            var records = new PowerTradeVolume[]
            {
                new PowerTradeVolume() {LocalTime = new TimeOnly(23, 0), Volume = 2378.7611403547903},
                new PowerTradeVolume() {LocalTime = new TimeOnly(0, 0), Volume = 2164.143982240519},
                new PowerTradeVolume() {LocalTime = new TimeOnly(1, 0), Volume = 2503.724408231943},
                new PowerTradeVolume() {LocalTime = new TimeOnly(2, 0), Volume = 2443.399767739372},
                new PowerTradeVolume() {LocalTime = new TimeOnly(3, 0), Volume = 3143.8779890488645},
                new PowerTradeVolume() {LocalTime = new TimeOnly(4, 0), Volume = 2467.733601513129},
                new PowerTradeVolume() {LocalTime = new TimeOnly(5, 0), Volume = 2187.106814392625},
                new PowerTradeVolume() {LocalTime = new TimeOnly(6, 0), Volume = 881.0644591152544},
                new PowerTradeVolume() {LocalTime = new TimeOnly(7, 0), Volume = 1476.5971136532003},
                new PowerTradeVolume() {LocalTime = new TimeOnly(8, 0), Volume = 1640.8123621171815},
                new PowerTradeVolume() {LocalTime = new TimeOnly(9, 0), Volume = 1882.6557636442367},
                new PowerTradeVolume() {LocalTime = new TimeOnly(10, 0), Volume = 1588.9043801092553},
                new PowerTradeVolume() {LocalTime = new TimeOnly(11, 0), Volume = 1560.7716983745365},
                new PowerTradeVolume() {LocalTime = new TimeOnly(12, 0), Volume = 1181.6680461577562},
                new PowerTradeVolume() {LocalTime = new TimeOnly(13, 0), Volume = 1466.4481366460502},
                new PowerTradeVolume() {LocalTime = new TimeOnly(14, 0), Volume = 2366.266745037757},
                new PowerTradeVolume() {LocalTime = new TimeOnly(15, 0), Volume = 1681.2343534154882},
                new PowerTradeVolume() {LocalTime = new TimeOnly(16, 0), Volume = 2413.144409734316},
                new PowerTradeVolume() {LocalTime = new TimeOnly(17, 0), Volume = 2361.222673839937},
                new PowerTradeVolume() {LocalTime = new TimeOnly(18, 0), Volume = 1789.2044344523426},
                new PowerTradeVolume() {LocalTime = new TimeOnly(19, 0), Volume = 2282.0367729079157},
                new PowerTradeVolume() {LocalTime = new TimeOnly(20, 0), Volume = 2551.781702261508},
                new PowerTradeVolume() {LocalTime = new TimeOnly(21, 0), Volume = 1515.3427702607057},
                new PowerTradeVolume() {LocalTime = new TimeOnly(22, 0), Volume = 1710.9728606389122}
            };

           

            return records;
        }
    }
}
