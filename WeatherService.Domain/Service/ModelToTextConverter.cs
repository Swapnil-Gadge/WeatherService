using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using WeatherService.Data.Model;

namespace WeatherService.Domain
{
    public class ModelToTextConverter : IModelToTextConverter
    {
        public MemoryStream ConvertModelToText(ConcurrentBag<WeatherDetailsModel> list)
        {
            string weatherData = string.Empty;

            if (!list.Any())
                return new MemoryStream(Encoding.ASCII.GetBytes(weatherData));

            weatherData = "Location\tLongitude\tLatitude\tWeather Condition\tTemperature\tFeels Like\tMinimum\tMaximum\tPressure\tHumidity\tVisibility\tWind Speed\tWind Direction in Degrees\tCloudiness %\n";
            foreach (var model in list)
            {
                weatherData += model.Name.ToString() + "\t";
                weatherData += model.Coord.Lon.ToString() + "\t";
                weatherData += model.Coord.Lat.ToString() + "\t";
                weatherData += model.Weather.FirstOrDefault().Description.ToString() + "\t";
                weatherData += model.Main.Temp.ToString() + "\t";
                weatherData += model.Main.Feels_Like.ToString() + "\t";
                weatherData += model.Main.Temp_Min.ToString() + "\t";
                weatherData += model.Main.Temp_Max.ToString() + "\t";
                weatherData += model.Main.Pressure.ToString() + "\t";
                weatherData += model.Main.Humidity.ToString() + "\t";
                weatherData += model.Visibility.ToString() + "\t";
                weatherData += model.Wind.Speed.ToString() + "\t";
                weatherData += model.Wind.Deg.ToString() + "\t";
                weatherData += model.Clouds.All.ToString() + "\n";                
            }

            return new MemoryStream(Encoding.ASCII.GetBytes(weatherData));
        }
    }
}
