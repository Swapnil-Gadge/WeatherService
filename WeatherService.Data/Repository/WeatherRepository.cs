using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Data.Client;
using WeatherService.Data.Model;

namespace WeatherService.Data.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly HttpClient httpClient;
        private readonly IMemoryCache memoryCache;
        private readonly ConcurrentBag<WeatherDetailsModel> outputList = new ConcurrentBag<WeatherDetailsModel>();
        private readonly string WeatherAPIPath = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid=aa69195559bd4f88d79f9aadeb77a8f6&units=metric";
        public WeatherRepository(IHttpHandler handler, IMemoryCache memoryCache)
        {
            this.httpClient = handler.Client;
            this.memoryCache = memoryCache;
        }

        public async Task<ConcurrentBag<WeatherDetailsModel>> GetWeatherDataForCities(string[] listOfCities)
        {
            Parallel.ForEach(listOfCities, (city) =>
            {
                var cityName = city.Trim().ToLowerInvariant();
                WeatherDetailsModel weatherDetailsForCity = null;

                if (this.memoryCache == null)
                    weatherDetailsForCity = GetWeatherDataForCity(cityName).Result;
                else
                {
                    weatherDetailsForCity = this.memoryCache.GetOrCreateAsync(cityName, entry =>
                    {
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                        var weatherData = GetWeatherDataForCity(cityName).Result;
                        return Task.FromResult(weatherData);
                    }).Result;
                }                

                if (weatherDetailsForCity != null)
                {
                    this.outputList.Add(weatherDetailsForCity);
                }
            });

            return this.outputList;
        }

        private async Task<WeatherDetailsModel> GetWeatherDataForCity(string cityName)
        {
            WeatherDetailsModel result = null;
            string requestUri = string.Format(WeatherAPIPath, cityName);
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                string weatherDetails = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<WeatherDetailsModel>(weatherDetails);
            }
            return result;
        }
    }
}
