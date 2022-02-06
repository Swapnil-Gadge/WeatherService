using System.Collections.Concurrent;
using System.IO;
using WeatherService.Data.Model;

namespace WeatherService.Domain
{
    public interface IModelToTextConverter
    {
        MemoryStream ConvertModelToText(ConcurrentBag<WeatherDetailsModel> list);
    }
}
