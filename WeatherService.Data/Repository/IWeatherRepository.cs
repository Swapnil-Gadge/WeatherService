using System.Collections.Concurrent;
using System.Threading.Tasks;
using WeatherService.Data.Model;

namespace WeatherService.Data.Repository
{
    public interface IWeatherRepository
    {
        Task<ConcurrentBag<WeatherDetailsModel>> GetWeatherDataForCities(string[] listOfCities);
    }
}
