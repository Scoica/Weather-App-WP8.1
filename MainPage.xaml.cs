using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Net.Http;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private const double kelvin = 273.15;
        private Windows.Storage.ApplicationDataContainer localHistory;
        private DateTime today;

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + textBox_City.Text + "," + textBox_Country.Text + "&appid=3ff1f9df1e3920b455fd1a63e5b7baea";

            HttpClient client = new HttpClient();
            string stringDate = await client.GetStringAsync(new Uri(url));

            RootObject data = JsonConvert.DeserializeObject<RootObject>(stringDate);

            result_Min.Text = "Min : " + ((int)(data.main.temp_min - kelvin)).ToString() + " C°";
            result_Max.Text = "Max : " + ((int)(data.main.temp_max - kelvin)).ToString() + " C°";
            result_Weather.Text = data.weather[0].description;
            result_City.Text = data.name;
            result_Humidity.Text = "Humidity " + data.main.humidity.ToString() + "%";
            result_Pressure.Text = "Pressure " + data.main.pressure.ToString() + " hPa";
            textBlock_Lat.Text = data.coord.lat.ToString();
            textBlock_Lon.Text = data.coord.lon.ToString();

            textBox_City.Text = defaultCity;
            textBox_Country.Text = defaultCountry;

            today = DateTime.Now;
            localHistory = Windows.Storage.ApplicationData.Current.LocalSettings;
            localHistory.Values[today.ToString() ] = data.name;
        }

        private async void buttonLocation_Click(object sender, RoutedEventArgs e)
        {
            Geolocator geoLocator = new Geolocator();
            geoLocator.DesiredAccuracyInMeters = 50;

            Geoposition pos = await geoLocator.GetGeopositionAsync();
            //await Task.Delay(TimeSpan.FromSeconds(4));

            string url = "http://api.openweathermap.org/data/2.5/weather?lat=" + pos.Coordinate.Latitude + "&lon=" + pos.Coordinate.Longitude + "&appid=3ff1f9df1e3920b455fd1a63e5b7baea";
            textBlock_Lat.Text = pos.Coordinate.Latitude.ToString();
            textBlock_Lon.Text = pos.Coordinate.Longitude.ToString();

            try
            {
                HttpClient client = new HttpClient();
                string stringDate = await client.GetStringAsync(new Uri(url));

                RootObject data = JsonConvert.DeserializeObject<RootObject>(stringDate);

                result_Min.Text = "Min : " + ((int)(data.main.temp_min - kelvin)).ToString() + " C°";
                result_Max.Text = "Max : " + ((int)(data.main.temp_max - kelvin)).ToString() + " C°";
                result_Weather.Text = data.weather[0].description;
                result_City.Text = data.name;
                result_Humidity.Text = "Humidity " + data.main.humidity.ToString() + "%";
                result_Pressure.Text = "Pressure " + data.main.pressure.ToString() + " hPa";
            }
            catch (HttpRequestException httpException)
            {
                Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog("Internet connection is off." + httpException.ToString());
                await msg.ShowAsync();
            }
        }

        private const string defaultCity = "Enter City";
        private void textBox_City_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox_City.Text = textBox_City.Text == defaultCity ? string.Empty : textBox_City.Text;
        }

        private void textBox_City_LostFocus(object sender, RoutedEventArgs e)
        {
            textBox_City.Text = textBox_City.Text == string.Empty ? defaultCity : textBox_City.Text;
        }

        private const string defaultCountry = "Enter Country";
        private void textBox_Country_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox_Country.Text = textBox_Country.Text == defaultCountry ? string.Empty : textBox_Country.Text;
        }

        private void textBox_Country_LostFocus(object sender, RoutedEventArgs e)
        {
            textBox_Country.Text = textBox_Country.Text == string.Empty ? defaultCountry : textBox_Country.Text;
        }

        private void button_Storage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HistoryPage));
        }
    }
}
