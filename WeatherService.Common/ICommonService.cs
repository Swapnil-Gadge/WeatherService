using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Common
{
    public interface ICommonService
    {
        (double?, double?) GetCityLatAndLong(string cityName);
    }
}
