using System.Net.Http;

namespace WeatherService.Data.Client
{
    public interface IHttpHandler
    {
        HttpClient Client { get; }
    }
}
