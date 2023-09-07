using Microsoft.AspNetCore.Mvc;
using WeatherService.Models;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Xml.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Service;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherServiceController : ControllerBase
    {
        private const string UrlAPILocation = "https://api.open-meteo.com/v1/forecast?";
        private const string UrlAPICity = "https://geocoding-api.open-meteo.com/v1/search?";
        private readonly weatherLocationServices _locationService;
        private readonly weatherCityServices _cityService;

        public WeatherServiceController(weatherLocationServices locationService, weatherCityServices CityService)
        {
            _locationService = locationService;
            _cityService = CityService;
        }

        [HttpGet()]
        [Produces("application/json")]
        public async Task<ActionResult<WeatherResponse>> GetLocation(double latitude, double longitude)
        {
            try
            {
                //take the last record in the last hour, if doen't exist get record from api
                var location = (await _locationService.GetAsync(latitude, longitude, DateTime.UtcNow.AddHours(-1))).ToList().FirstOrDefault();
                if (location == null)
                    location = await ReadAPIAsync(latitude, longitude);

                return Ok(CreateResponse(location));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("GetWeatherCity")]
        public async Task<ActionResult<WeatherResponse>> GetCity(string city)
        {
            try
            {
                //Get location of the city
                double Latitude = 0;
                double Longitude = 0;

                var citylocation = (await _cityService.GetAsync(city.ToUpper())).ToList().FirstOrDefault();
                if (citylocation == null)
                {
                    var responseCity = await ReadAPICityAsync(city);
                    Latitude = responseCity.Latitude;
                    Longitude = responseCity.Longitude;
                }
                else
                {
                    Latitude = citylocation.Latitude;
                    Longitude = citylocation.Longitude;
                }

                //take the last record in the last hour, if doen't exist get record from api
                var location = (await _locationService.GetAsync(Latitude, Longitude, DateTime.UtcNow.AddHours(-1))).ToList().FirstOrDefault();
                if (location == null)
                    location = await ReadAPIAsync(Latitude, Longitude);

                return Ok(CreateResponse(location));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        //Read Api location and save data en mongoDB
        private async Task<WeatherLocation> ReadAPIAsync(double latitude, double longitude)
        {

            // Create an HttpClient instance.
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Format the Api url  
                    string apiUrl = string.Format("{0}latitude={1}&longitude={2}&daily=sunrise&current_weather=true&timezone=auto&forecast_days=1", UrlAPILocation, latitude.ToString(), longitude.ToString());
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string.
                        string responseBody = await response.Content.ReadAsStringAsync();
                        WeatherDataEntity weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherDataEntity>(responseBody);

                        if (weatherData != null)
                        {
                            //Create new location object
                            WeatherLocation objectResponse = new WeatherLocation();
                            objectResponse.Latitude = latitude;
                            objectResponse.Longitude = longitude;
                            objectResponse.Generationtime = DateTime.UtcNow;
                            objectResponse.Time = weatherData.Current_weather.Time;
                            objectResponse.Wind_Direction = weatherData.Current_weather.Winddirection;
                            objectResponse.Wind_Speed = weatherData.Current_weather.Windspeed;
                            objectResponse.Tempeture = weatherData.Current_weather.Temperature;
                            objectResponse.Sunrise = weatherData.Daily.Sunrise.FirstOrDefault();

                            //Save location object
                            await _locationService.CreateAsync(objectResponse);

                            return objectResponse;
                        }
                        else
                        {
                            throw new Exception("Failed to deserialize API response.");
                        }

                    }
                    else
                    {
                        throw new Exception($"API request failed with status code {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }

        //get location of the city
        private async Task<CityLocation> ReadAPICityAsync(string city)
        {

            // Create an HttpClient instance.
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    // Format the Api url  
                    string apiUrl = string.Format("{0}name={1}&count=1&language=es&format=json", UrlAPICity, city);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string.
                        string responseBody = await response.Content.ReadAsStringAsync();
                        CityHeader LocationData = Newtonsoft.Json.JsonConvert.DeserializeObject<CityHeader>(responseBody);

                        if (LocationData.Results != null)
                        {
                            var cityLocation = LocationData.Results.First();
                            WeatherCity objectResponse = new WeatherCity();
                            objectResponse.Name = cityLocation.Name;
                            objectResponse.Latitude = cityLocation.Latitude;
                            objectResponse.Longitude = cityLocation.Longitude;

                            await _cityService.CreateAsync(objectResponse);
                            return new CityLocation(cityLocation.Latitude, cityLocation.Longitude);
                        }
                        else
                        {
                            throw new Exception("Failed to deserialize API response.");
                        }
                    }
                    else
                    {
                        throw new Exception($"API request failed with status code {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }

        //Create object response
        private WeatherResponse CreateResponse(WeatherLocation responseWeather)
        {
            return new WeatherResponse(responseWeather.Wind_Direction, responseWeather.Wind_Speed, responseWeather.Tempeture, responseWeather.Sunrise);
        }

    }
}