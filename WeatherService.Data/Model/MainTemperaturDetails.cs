using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Data.Model
{
    public class MainTemperaturDetails
    {
        public double Temp { get; set; }
        public double Feels_Like { get; set; }
        public double Temp_Min { get; set; }
        public double Temp_Max { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
    }
}
