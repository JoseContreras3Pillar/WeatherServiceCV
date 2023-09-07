using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace WeatherService.Models
{
    public class WeatherCity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
