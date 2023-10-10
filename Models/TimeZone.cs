using System;

namespace P04WeatherForecastAPI.Client.Models;

public class TimeZone
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double GmtOffset { get; set; }
    public bool IsDaylightSaving { get; set; }
}