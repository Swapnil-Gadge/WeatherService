using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using WeatherService.Common;
using WeatherService.Domain.Service;

namespace WeatherService.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/weather-data")]
    public class WeatherController : Controller
    {
        private readonly IWeatherService weatherService;
        public WeatherController(IWeatherService weatherService) 
        {
            this.weatherService = weatherService;
        }

        [HttpGet]
        [Route("{cities?}")]
        public async Task<IActionResult> GetWeatherDataForCities([FromRoute]string cities=null)
        {   
            if (string.IsNullOrWhiteSpace(cities))
                return NotFound();

            return await this.FetchWeatherDataForCities(cities);
        }

        [HttpPost]
        public async Task<IActionResult> GetWeatherDataForCities()
        {
            var file = this.Request.Form.Files[0];
            var cities = file.ReadFileData();

            if (string.IsNullOrWhiteSpace(cities))
                return NotFound();

            return await this.FetchWeatherDataForCities(cities);

        }

        private async Task<FileStreamResult> FetchWeatherDataForCities(string cities)
        {
            var stream = await this.weatherService.GetWeatherDataForCities(cities);
            return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = $"WeatherData-{DateTime.UtcNow.ToString("d")}.txt"
            };
        }
    }
}