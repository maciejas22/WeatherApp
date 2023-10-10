using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using P04WeatherForecastAPI.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace P04WeatherForecastAPI.Client.Services
{
    internal class AccuWeatherService
    {
        private const string base_url = "http://dataservice.accuweather.com";
        private const string autocomplete_endpoint = "locations/v1/cities/autocomplete?apikey={0}&q={1}&language{2}";
        private const string current_conditions_endpoint = "currentconditions/v1/{0}?apikey={1}&language{2}";
        
        private const string hourly_forecast_endpoint = "forecasts/v1/hourly/12hour/{0}?apikey={1}&language{2}&metric=true";
        private const string historical_hourly_forecast_endpoint = "currentconditions/v1/{0}/historical?apikey={1}";
        private const string five_days_forecast_endpoint = "forecasts/v1/daily/5day/{0}?apikey={1}&language{2}&metric=true";
        private const string five_days_indices_endpoint = "indices/v1/daily/5day/{0}?apikey={1}&language{2}";
        private const string location_information_endpoint = "locations/v1/{0}?apikey={1}&language{2}";
        private const string location_neightbors_endpoint = "locations/v1/cities/neighbors/{0}?apikey={1}&language{2}";
        
        // private const string api_key = "5hFl75dja3ZuKSLpXFxUzSc9vXdtnwG5";
        string api_key;
        //private const string language = "pl";
        string language;

        public AccuWeatherService()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetings.json"); 

            var configuration = builder.Build();
            api_key = configuration["api_key"];
            language = configuration["default_language"] ?? "pl";
        }


        public async Task<City[]> GetLocations(string locationName)
        {
            var uri = base_url + "/" + string.Format(autocomplete_endpoint, api_key, locationName, language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var cities = JsonConvert.DeserializeObject<City[]>(json);
                return cities;

            }
        }

        public async Task<double> GetCurrentTemp(string cityKey)
        {
            var uri = base_url + "/" + string.Format(current_conditions_endpoint, cityKey, api_key,language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var weathers= JsonConvert.DeserializeObject<Weather[]>(json);
                return weathers.FirstOrDefault().Temperature.Metric.Value;
            }
        }

        public async Task<float[]> GetHourlyForecast(string cityKey)
        {
            var uri = base_url + "/" + string.Format(hourly_forecast_endpoint, cityKey, api_key,language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var weathers= JsonConvert.DeserializeObject<Weather[]>(json);

                var temps = new float[12];
                for (int i = 0; i < weathers.Length; i++)
                {
                    temps[i] = weathers[i].Temperature.Value;
                }

                return temps;
            }       
        }

        public async Task<(float[], float[])> GetDailyForecast(string cityKey)
        {
            var uri = base_url + "/" + string.Format(five_days_forecast_endpoint, cityKey, api_key,language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var weathers= JsonConvert.DeserializeObject<FiveDayForecast>(json);

                var minTemp = new float[5];
                var maxTemp = new float[5];
                for (int i = 0; i < weathers.DailyForecasts.Count; i++)
                {
                    minTemp[i] = weathers.DailyForecasts[i].Temperature.Minimum.Value;
                    maxTemp[i] = weathers.DailyForecasts[i].Temperature.Maximum.Value;
                }

                return (minTemp, maxTemp);
            }    
        }

        public async Task<string[]> GetRunningIndices(string cityKey)
        {
            var uri = base_url + "/" + string.Format(five_days_indices_endpoint, cityKey, api_key,language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var indices= JsonConvert.DeserializeObject<Indicies[]>(json);

                var runningIndices = indices
                    .Where(elem => elem.ID == 1)
                    .Select(elem => elem.Category)
                    .ToArray();
                return runningIndices;
            }
        }

        public async Task<LocationInfo> GetLocationInfo(string cityKey)
        {
            var uri = base_url + "/" + string.Format(location_information_endpoint, cityKey, api_key,language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var locationInfo = JsonConvert.DeserializeObject<LocationInfo>(json);

                return locationInfo;
            }
        }

        public async Task<string[]> GetLocationNeightbours(string cityKey)
        {
            var uri = base_url + "/" + string.Format(location_neightbors_endpoint, cityKey, api_key, language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var locationNeightbours = JsonConvert.DeserializeObject<LocationNeightbour[]>(json);

                var neightbours = locationNeightbours.Select(elem => elem.EnglishName).ToArray();
                return neightbours;
            }
        }

        public async Task<double[]> GetHistoricalHourlyForeCast(string cityKey)
        {
            var uri = base_url + "/" + string.Format(historical_hourly_forecast_endpoint, cityKey, api_key, language);
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var weathers= JsonConvert.DeserializeObject<Weather[]>(json);

                var temps = new double[6];
                for (var i = 0; i < weathers.Length; i++)
                {
                    temps[i] = weathers[i].Temperature.Metric.Value;
                }

                return temps;
            }     
        }

    }
}
