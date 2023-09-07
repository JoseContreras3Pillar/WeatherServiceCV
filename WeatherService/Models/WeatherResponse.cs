using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace WeatherService.Models
{
    public class WeatherResponse
    {
        public WeatherResponse(double wind_Direction, double wind_Speed, double tempeture, DateTime sunrise) {
            Wind_Direction = wind_Direction;
            Wind_Speed = wind_Speed; 
            Tempeture = tempeture;
            Sunrise = sunrise;
        }

        public double Wind_Direction { get; set; }
        public double Wind_Speed { get; set; }
        public double Tempeture { get; set; }
        public DateTime Sunrise { get; set; }
    }

}
