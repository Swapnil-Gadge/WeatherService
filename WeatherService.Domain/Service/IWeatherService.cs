using System.IO;
using System.Threading.Tasks;

namespace WeatherService.Domain.Service
{
    public interface IWeatherService
    {
        Task<MemoryStream> GetWeatherDataForCities(string listOfCities);
    }
}
