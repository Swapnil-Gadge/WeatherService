using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherService.Controllers;
using WeatherService.Data.Client;
using WeatherService.Data.Repository;
using WeatherService.Domain;
using WeatherService.Domain.Service;

namespace WeatherService.Unit
{
    public class WeatherServiceControllerTests
    {
        public WeatherController weatherController;

        [Test]
        public async Task GivenValidCityNames_WhenGetWeatherDataForCitiesByInputfile_ThenShouldGetBackValidData()
        {
            // Arrange
            this.SetupContext(weatherData: TestConstant.WeatherData.London);

            // Act
            this.weatherController.ControllerContext = this.RequestWithFile("London");
            var actionResult = await this.weatherController.GetWeatherDataForCities();
            var result = actionResult as FileStreamResult;

            // Assert
            Assert.NotNull(result);

            string contents;
            using (var sr = new StreamReader(result.FileStream))
            {
                contents = sr.ReadToEnd();
            }

            Assert.IsTrue(contents.ToLowerInvariant().IndexOf("london") >= 0);
        }

        [Test]
        public async Task GivenNoCityNames_WhenGetWeatherDataForCitiesByInputfile_ThenShouldGetBackNotFoundResult()
        {
            // Arrange
            this.SetupContext(weatherData: TestConstant.WeatherData.London);

            // Act
            this.weatherController.ControllerContext = this.RequestWithFile(string.Empty);
            var actionResult = await this.weatherController.GetWeatherDataForCities();
            var result = actionResult as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GivenValidCityNames_WhenGetWeatherDataForCities_ThenShouldGetBackValidData()
        {
            await ExecuteTest("London", 2,  weatherData: TestConstant.WeatherData.London);
        }

        [Test]
        public async Task GivenEmptyNullCityNames_WhenGetWeatherDataForCities_ThenShouldGet404NotFound()
        {
            SetupContext();
            string cityName = string.Empty;
            var actionResult = await this.weatherController.GetWeatherDataForCities(cityName);

            Assert.IsTrue(actionResult is NotFoundResult);            
        }

        [Test]
        public async Task GivenInValidCityNamesOrNotSupported_WhenGetWeatherDataForCities_ThenShouldGetBackEmptyResult()
        {
            await ExecuteTest("London123|GandhiNagar");
        }

        [Test]
        public async Task GivenValidCityNameButStatusCodeFromServiceNotOk_WhenGetWeatherDataForCities_ThenShouldGetBackEmptyResult()
        {
            await ExecuteTest("London", 0, HttpStatusCode.NotFound);
        }

        private async Task ExecuteTest(
            string cities, 
            int resultCount = 0, 
            HttpStatusCode statusCode= HttpStatusCode.OK,
            string weatherData = null)
        {
            // Arrange
            SetupContext(statusCode, weatherData);

            // Act
            var actionResult = await this.weatherController.GetWeatherDataForCities(cities);
            var result = actionResult as FileStreamResult;

            // Assert
            Assert.NotNull(result);

            string contents;
            using (var sr = new StreamReader(result.FileStream))
            {
                contents = sr.ReadToEnd();
            }

            if (resultCount == 0)
                Assert.IsTrue(string.IsNullOrWhiteSpace(contents));
            else
            {
                Assert.IsTrue(!string.IsNullOrWhiteSpace(contents));
                foreach (var city in cities.Split("|"))
                {
                    Assert.IsTrue(contents.ToLowerInvariant().IndexOf(city.ToLowerInvariant()) >= 0);
                }
            }                
        }

        private void SetupContext(HttpStatusCode statusCode = HttpStatusCode.OK, string weatherData = null)
        {
            HttpClient httpClient;
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(weatherData == null ? string.Empty : weatherData),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            // Mock Http Client
            httpClient = new HttpClient(handlerMock.Object);
            var handler = new HttpHandler(httpClient);
            var weatherRepo = new WeatherRepository(handler, null);
            var weatherService = new WeathersService(weatherRepo, new ModelToTextConverter());
            this.weatherController = new WeatherController(weatherService);
        }

        //Add the file in the underlying request object.
        private ControllerContext RequestWithFile(string content)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)), 0, 1000, "Data", "dummy.txt");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            return new ControllerContext(actx);
        }
    }
}