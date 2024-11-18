using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MeteoV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiKey = "16a01e77b1d33e51fa848496f1e5c5a6"; // Remplacez par votre clé API
        private const string city = "Annecy";
        private const string country = "FR";

        public MainWindow()
        {
            InitializeComponent();
            RefreshWeather();
        }

        private async void RefreshWeather()
        {
            try
            {
                var weatherData = await GetWeatherDataAsync();
                TemperatureText.Text = $"Température: {weatherData.Item1} °C";
                ConditionsText.Text = $"Conditions: {weatherData.Item2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}");
            }
        }

        private async Task<Tuple<double, string>> GetWeatherDataAsync()
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid={apiKey}&units=metric";
            var response = await client.GetStringAsync(url);
            var data = JObject.Parse(response);

            double temperature = data["main"]["temp"].ToObject<double>();
            string conditions = data["weather"][0]["description"].ToString();

            return Tuple.Create(temperature, conditions);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshWeather();
        }
    }
}