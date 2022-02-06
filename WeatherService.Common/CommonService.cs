using System.Collections.Concurrent;

namespace WeatherService.Common
{
    public class CommonService : ICommonService
    {
        // Multiple threads should be able to read data from dictionary 
        private ConcurrentDictionary<string, (double?, double?)> CitiesCoordinates = new ConcurrentDictionary<string, (double?, double?)>();

        // TODO: This code can be eliminated. Didn't know we can fetch data based on city names as well. Though Lat and Long are mandatory.
        public CommonService()
        {
            CitiesCoordinates.TryAdd(Constants.Cities.London, (51.5072, 0.1276));
            CitiesCoordinates.TryAdd(Constants.Cities.Paris, (48.8566, 2.3522));
            CitiesCoordinates.TryAdd(Constants.Cities.Dublin, (53.3498, 6.2603));
            CitiesCoordinates.TryAdd(Constants.Cities.Washington, (47.7511, 120.7401));
            CitiesCoordinates.TryAdd(Constants.Cities.NewYork, (40.7128, 74.0060));
            CitiesCoordinates.TryAdd(Constants.Cities.Delhi, (28.7041, 77.1025));
            CitiesCoordinates.TryAdd(Constants.Cities.Mumbai, (19.0760, 72.8777));
            CitiesCoordinates.TryAdd(Constants.Cities.Rome, (41.9028, 12.4964));
            CitiesCoordinates.TryAdd(Constants.Cities.Berlin, (52.5200, 13.4050));
            CitiesCoordinates.TryAdd(Constants.Cities.Dubai, (25.2048, 55.2708));
        }

        public (double?, double?) GetCityLatAndLong(string cityName)
        {
            return CitiesCoordinates.TryGetValue(cityName.ToLowerInvariant().Trim(), out (double?, double?) value) ? value : (null, null);
        }
    }
}
