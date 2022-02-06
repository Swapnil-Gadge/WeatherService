using System.IO;
using System.Threading.Tasks;
using WeatherService.Data.Repository;

namespace WeatherService.Domain.Service
{
    public class WeathersService : IWeatherService
    {
        public readonly IWeatherRepository weatherRepository;
        public readonly IModelToTextConverter converter;

        public WeathersService(IWeatherRepository weatherRepository, IModelToTextConverter converter)
        {
            this.weatherRepository = weatherRepository;
            this.converter = converter;
        }

        public async Task<MemoryStream> GetWeatherDataForCities(string listOfCities)
        {
            var result =  await this.weatherRepository.GetWeatherDataForCities(listOfCities.Split('|'));
            return converter.ConvertModelToText(result);
        }
    }
}
