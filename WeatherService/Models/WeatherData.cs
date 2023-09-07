using System;
using System.Collections.Generic;

namespace WeatherService.Models
{
    public class WeatherDataEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Generationtime_ms { get; set; }
        public int Utc_offset_seconds { get; set; }
        public string Timezone { get; set; }
        public string Timezone_abbreviation { get; set; }
        public double Elevation { get; set; }
        public CurrentWeatherEntity Current_weather { get; set; }
        public DailyUnitsEntity Daily_units { get; set; }
        public DailyDataEntity Daily { get; set; }
    }

}
