using System;

namespace P04WeatherForecastAPI.Client.Models;

public class Indicies
{
    public string Name { get; set; }
    public int ID { get; set; }
    public bool Ascending { get; set; }
    public DateTime LocalDateTime { get; set; }
    public int EpochDateTime { get; set; }
    public float Value { get; set; }
    public string Category { get; set; }
    public int CategoryValue { get; set; }
    public string MobileLink { get; set; }
    public string Link { get; set; }
}