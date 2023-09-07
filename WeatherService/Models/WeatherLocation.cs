using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace WeatherService.Models
{
    public class WeatherLocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Time { get; set; }
        public DateTime Generationtime { get; set; }
        public double Wind_Direction { get; set; }
        public double Wind_Speed { get; set; }
        public double Tempeture { get; set; }
        public DateTime Sunrise { get; set; }
    }

}
