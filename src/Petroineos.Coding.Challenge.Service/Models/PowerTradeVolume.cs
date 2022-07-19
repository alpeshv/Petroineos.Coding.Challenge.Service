using CsvHelper.Configuration.Attributes;

namespace Petroineos.Coding.Challenge.Service.Models
{
    public class PowerTradeVolume
    {
        [Index(0)]
        [Name("Local Time")]
        public TimeOnly LocalTime { get; set; }

        [Index(1)]
        [Name("Volume")]
        public double Volume { get; set; }
    }
}
