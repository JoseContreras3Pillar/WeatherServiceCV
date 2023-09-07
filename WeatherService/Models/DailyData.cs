using System;
using System.Collections.Generic;

namespace WeatherService.Models
{
    public class DailyDataEntity
    {
        public List<DateTime> Time { get; set; }
        public List<DateTime> Sunrise { get; set; }
    }
}
