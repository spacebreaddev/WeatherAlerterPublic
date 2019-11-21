using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeatherAlerter.Models;

namespace WeatherAlerter
{
    static class OpenWeatherAPIRequests
    {
        //api call
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        static HttpClient client = new HttpClient();
        static private string _apiAddress = ConfigurationManager.AppSettings.Get("WeatherAddress");
        static private string _apiKey = ConfigurationManager.AppSettings.Get("WeatherAPIKey");
        static private string _city = HttpUtility.HtmlEncode(ConfigurationManager.AppSettings.Get("WeatherCity"));

        static public async Task<FiveDay> GetFiveDayForecast()
        {
            var product = await GetProductAsync(String.Format(_apiAddress, _city, _apiKey));

            return JsonConvert.DeserializeObject<FiveDay>(product);
        }

        static async Task<string> GetProductAsync(string path)
        {
            string product = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }
            return product;
        }

    }
}
