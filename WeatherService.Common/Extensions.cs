using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace WeatherService.Common
{
    public static class Extensions
    {
        public static string ReadFileData(this IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            return result.ToString();
        }
    }
}
