using System.Collections.Generic;

namespace WeatherService.Data.Model
{
    public class WeatherDetailsModel
    {
        public CoOrdinateDetails Coord { get; set; }
        public List<WeatherDetails> Weather { get; set; }
        public MainTemperaturDetails Main { get; set; }
        public long Visibility { get; set; }
        public WindDetails Wind { get; set; }
        public Cloudiness Clouds { get; set; }
        public long Dt { get; set; }
        public int TimeZone { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
