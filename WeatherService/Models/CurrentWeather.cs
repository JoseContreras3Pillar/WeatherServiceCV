using System;
using System.Collections.Generic;

namespace WeatherService.Models
{
    public class CurrentWeatherEntity
    {
        public double Temperature { get; set; }
        public double Windspeed { get; set; }
        public int Winddirection { get; set; }
        public int Weathercode { get; set; }
        public int Is_day { get; set; }
        public DateTime Time { get; set; }
    }
}
