using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelpingFarmerBot
{
    public class WeatherInfoReader
    {

        public String getweather(String messagetext)
        {
            var city = messagetext.Split(':')[1].Trim();

            try
            {
                var apiKey = "c38e770a1da2d9c102a1e4a5ae98f0d1";
                var newUrl = $"http://api.openweathermap.org/data/2.5/weather?appid={apiKey}&q={city}";

                var weatherJsonMsg = this.HttpGet(newUrl);
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(weatherJsonMsg);

                var temperature = ConvertKelvinToCelcius(weatherData.Main["temp"]);
                var minTemperature = ConvertKelvinToCelcius(weatherData.Main["temp_min"]);
                var maxTemperature = ConvertKelvinToCelcius(weatherData.Main["temp_max"]);

                var toReturnString = $"Temperature in {city.ToUpperInvariant()} is : {temperature} C ; Min Temp: {minTemperature} C; Max Temp: {maxTemperature} C";
                return toReturnString;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return ($"Weather data for {city.ToUpperInvariant()} is unavailable.");
            }
        }

        private string ConvertKelvinToCelcius(string kelvin)
        {
            var tempInKelvin = Convert.ToDouble(kelvin);
            return (tempInKelvin - 273.15).ToString();
        }

        public string HttpGet(string URI)
        {
            WebClient client = new WebClient();

            // Add a user agent header in case the 
            // requested URI contains a query.

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            Stream data = client.OpenRead(URI);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();

            return s;
        }

    }

}
