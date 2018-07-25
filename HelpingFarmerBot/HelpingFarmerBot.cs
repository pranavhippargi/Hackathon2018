using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using SimpleEchoBot.FarmingInfo;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelpingFarmerBot
{
    public class HelpingFarmerBot : IBot
    {
        private static string HelpMessage = $"Welcome to Infarmation. \nDiscover global crop prices and temperature. Try following options. \n Price: 'Crop' \n Weather: 'City'";
        /// <summary>
        /// Every Conversation turn for our HelpingFarmerBot will call this method. In here
        /// the bot checks the Activty type to verify it's a message, bumps the 
        /// turn conversation 'Turn' count, and then echoes the users typing
        /// back to them. 
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurn(ITurnContext context)
        {
            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:
                    var messagetext = context.Activity.Text.Trim().ToLower();

                    if (messagetext.ToLowerInvariant().Contains("weather"))
                    {
                        var city = messagetext.Split(':')[1].Trim();

                        try
                        {
                            var apiKey = "d0350e61d7e88eac4874c96c578cb95b";
                            var newUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city},uk&APPID={apiKey}";

                            var weatherJsonMsg = this.HttpGet(newUrl);
                            var weatherData = JsonConvert.DeserializeObject<WeatherData>(weatherJsonMsg);

                            var temperature = ConvertKelvinToCelcius(weatherData.Main["temp"]);
                            var minTemperature = ConvertKelvinToCelcius(weatherData.Main["temp_min"]);
                            var maxTemperature = ConvertKelvinToCelcius(weatherData.Main["temp_max"]);

                            await context.SendActivity($"Temperature in {city.ToUpperInvariant()} is : {temperature} C ; Min Temp: {minTemperature} C; Max Temp: {maxTemperature} C");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }

                    if (messagetext.Contains("price"))
                    {
                        var message = GetPrice(messagetext);
                        await context.SendActivity(message);
                    }
                    else
                    {
                        await context.SendActivity(HelpMessage);
                    }
                    break;
                case ActivityTypes.ConversationUpdate:
                    foreach (var newMember in context.Activity.MembersAdded)
                    {
                        if (newMember.Id != context.Activity.Recipient.Id)
                        {
                            await context.SendActivity(HelpMessage);
                        }
                    }
                    break;
            }

        }

        private string ConvertKelvinToCelcius(string kelvin)
        {
            var tempInKelvin = Convert.ToDouble(kelvin);
            return (tempInKelvin - 273.15).ToString();
        }

        private string GetPrice(string messagetext)
        {
            var defaultMessage = "Commodity price not found. Try - Price: 'Wheat'.";
            if (string.IsNullOrEmpty(messagetext))
            {
                return defaultMessage;
            }

            var list = messagetext.Split(":");
            var produce = list[1].Replace("[","").Replace("]","").Trim();

            CropInfoReader infoReader = new CropInfoReader();

            var crop = (Crop)Int32.Parse("1");
            CropInfo info = infoReader.GetCropInfo(crop);
            var logMessage = $"{info.name} price today - low: {info.lowPrice} average: {info.avgPrice} high: {info.highPrice}";

            return logMessage;

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
