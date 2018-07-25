using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Twilio;
using System.Collections.Generic;
using Twilio.Rest.Lookups.V1;

namespace HelpingFarmerBot
{
    public class HelpingFarmerBot : IBot
    {
        private static string HelpMessage = $"Welcome to Infarmation. \nDiscover global crop prices and local temperature. Try following options. \n Price: 'Crop' \n Weather: 'City'";
        CropInfoReader infoReader = new CropInfoReader();

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
                    var countryId = GetCountry(context.Activity.From.Id);

                    await context.SendActivity(countryId);

                    if (messagetext.ToLowerInvariant().Contains("weather"))
                    {
                        var city = messagetext.Split(':')[1].Trim();

                        try
                        {
                            var apiKey = "d0350e61d7e88eac4874c96c578cb95b";
                            var newUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city},uk&APPID={apiKey}";

                            var weatherJsonMsg = this.HttpGet(newUrl);
                            var weatherData = JsonConvert.DeserializeObject<WeatherData>(weatherJsonMsg);

                            var temperature = weatherData.Main["temp"];

                            await context.SendActivity($"Temp:{temperature}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }

                    if (messagetext.Contains("price"))
                    {
                        var message = GetPrice(messagetext, countryId);
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

        private string GetCountry(string number)
        {
            const string accountSid = "AC1061e132398b5dc01334d9ff9b63d58a";
            const string authToken = "9e6a04f6a51ef7d32858ac16f83c2727";

            TwilioClient.Init(accountSid, authToken);

            var type = new List<string> {
                "carrier"
            };


            var phoneNumber = PhoneNumberResource.Fetch(
                type: type,
                pathPhoneNumber: new Twilio.Types.PhoneNumber(number)

            );

            return phoneNumber.CountryCode;
        }

        private string GetPrice(string messagetext, string countryId)
        {
            var defaultMessage = "Commodity price not found. Try - Price: 'Wheat'.";
            if (string.IsNullOrEmpty(messagetext))
            {
                return defaultMessage;
            }

            var list = messagetext.Split(":");
            var cropName = list[1].Replace("[","").Replace("]","").Trim();
            CropInfo info = infoReader.GetCropInfo(cropName, countryId);
            var reply = $"{info.name} price today - low: {info.lowPrice} average: {info.avgPrice} high: {info.highPrice}";

            return reply;

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
