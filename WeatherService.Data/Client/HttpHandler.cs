using System;
using System.Net.Http;

namespace WeatherService.Data.Client
{
    public class HttpHandler : IHttpHandler
    {
        private HttpClient instance;
        private static readonly object obj = new object();

        public HttpHandler() { }

        public HttpHandler(HttpClient client) 
        {
            instance = client;
        }

        public HttpClient Client
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                            instance = new HttpClient();
                    }
                }
                return instance;
            }
        }
    }
}
