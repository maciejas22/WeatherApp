using System.Collections.Generic;

namespace P04WeatherForecastAPI.Client.Models;

public class FiveDayForecast
{
    public Headline Headline { get; set; }
    public List<DailyForecast> DailyForecasts { get; set; }
}