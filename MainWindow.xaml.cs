using P04WeatherForecastAPI.Client.Models;
using P04WeatherForecastAPI.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P04WeatherForecastAPI.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AccuWeatherService accuWeatherService;
        public MainWindow()
        {
            InitializeComponent();
            accuWeatherService = new AccuWeatherService();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            
            City[] cities= await accuWeatherService.GetLocations(txtCity.Text);

            // standardowy sposób dodawania elementów
            //lbData.Items.Clear();
            //foreach (var c in cities)
            //    lbData.Items.Add(c.LocalizedName);

            // teraz musimy skorzystac z bindowania danych bo chcemy w naszej kontrolce przechowywac takze id miasta 
            lbData.ItemsSource = cities;
        }

        
        
        private async void lbData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCity= (City)lbData.SelectedItem;
            lblCityName.Content = selectedCity.LocalizedName;
            
            var locationKey = selectedCity.Key;
            if (locationKey != null)
            {
                SetCurrentTemp(locationKey);
                SetHourlyTemps(locationKey);
                SetHistoricalHourlyTemps(locationKey);
                SetDailyTemps(locationKey);
                SetRunningIndicies(locationKey);
                SetLocationInfo(locationKey);
                SetLocationNeightbors(locationKey);
            }
            
        }

        private async void SetCurrentTemp(string locationKey)
        {
            var tempValue = await accuWeatherService.GetCurrentTemp(locationKey);
            lblTemperatureValue.Content = Convert.ToString(tempValue);
        }
        
        private async void SetHourlyTemps(string locationKey)
        {
            var temps = await accuWeatherService.GetHourlyForecast(locationKey);
            lbl1h.Content = Convert.ToString(temps[0]);
            lbl2h.Content = Convert.ToString(temps[1]);
            lbl3h.Content = Convert.ToString(temps[2]);
            lbl4h.Content = Convert.ToString(temps[3]);
            lbl5h.Content = Convert.ToString(temps[4]);
            lbl6h.Content = Convert.ToString(temps[5]);
        }
        
        private async void SetHistoricalHourlyTemps(string locationKey)
        {
            var temps = await accuWeatherService.GetHistoricalHourlyForeCast(locationKey);
            lbl_1h.Content = Convert.ToString(temps[0]);
            lbl_2h.Content = Convert.ToString(temps[1]);
            lbl_3h.Content = Convert.ToString(temps[2]);
            lbl_4h.Content = Convert.ToString(temps[3]);
            lbl_5h.Content = Convert.ToString(temps[4]);
            lbl_6h.Content = Convert.ToString(temps[5]);
        }

        private async void SetDailyTemps(string locationKey)
        {

            var (minTemp, maxTemp) = await accuWeatherService.GetDailyForecast(locationKey);
            lbl1d.Content = Convert.ToString(minTemp[0]) + " - " + Convert.ToString(maxTemp[0]);
            lbl2d.Content = Convert.ToString(minTemp[1]) + " - " + Convert.ToString(maxTemp[1]);
            lbl3d.Content = Convert.ToString(minTemp[2]) + " - " + Convert.ToString(maxTemp[2]);
            lbl4d.Content = Convert.ToString(minTemp[3]) + " - " + Convert.ToString(maxTemp[3]);
            lbl5d.Content = Convert.ToString(minTemp[4]) + " - " + Convert.ToString(maxTemp[4]);
            
        }

        private async void SetRunningIndicies(string locationKey)
        {
            var runningIndices = await accuWeatherService.GetRunningIndices(locationKey);
            lblRun1d.Content = Convert.ToString(runningIndices[0]);
            lblRun2d.Content = Convert.ToString(runningIndices[1]);
            lblRun3d.Content = Convert.ToString(runningIndices[2]);
            lblRun4d.Content = Convert.ToString(runningIndices[3]);
            lblRun5d.Content = Convert.ToString(runningIndices[4]);
        }

        private async void SetLocationInfo(string locationKey)
        {
            var locationInfo = await accuWeatherService.GetLocationInfo(locationKey);
            lblLocationName.Content = Convert.ToString(locationInfo.EnglishName);
            lblRegionName.Content = Convert.ToString(locationInfo.Region.EnglishName);
            lblAdministrativeAreaName.Content = Convert.ToString(locationInfo.AdministrativeArea.EnglishName);
            lblTimeZone.Content = "GMT" + Convert.ToString(locationInfo.TimeZone.GmtOffset);
        }

        private async void SetLocationNeightbors(string locationKey)
        {
            var locationNeighbors = await accuWeatherService.GetLocationNeightbours(locationKey);
            neightboursData.ItemsSource = locationNeighbors.Length > 0 ? locationNeighbors : "No neightbours";
        }
    }
}
