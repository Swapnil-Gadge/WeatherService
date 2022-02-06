using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Data.Model
{
    public class WeatherDetails
    {
        public int Id { get; set; }

        public string Main { get; set; }

        public string Description { get; set; }
    }
}
