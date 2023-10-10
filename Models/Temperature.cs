using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P04WeatherForecastAPI.Client.Models
{
    public class Temperature
    {
        public Metric Metric { get; set; }
        public Imperial Imperial { get; set; }
        
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
        
        public Minimum Minimum { get; set; }
        public Maximum Maximum { get; set; }
    }
}
