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
        private static string HelpMessage = $"Welcome to CropCast. \nDiscover global crop prices and local temperature. Try following options. \n Price: 'Crop' \n Weather: 'City'";
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

                    if (messagetext.ToLowerInvariant().Contains("weather"))
                    {
                        WeatherInfoReader weatherRead = new WeatherInfoReader();
                        await context.SendActivity(weatherRead.getweather(messagetext));
                    }
                    else if (messagetext.Contains("price"))
                    {
                        try
                        {
                            var message = GetPrice(messagetext, countryId);
                            await context.SendActivity(message);
                        } catch (Exception e)
                        {
                            await context.SendActivity(e.Message);
                        }

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


            try
            {
                var phoneNumber = PhoneNumberResource.Fetch(
                    type: type,
                    pathPhoneNumber: new Twilio.Types.PhoneNumber(number)

                );

                return phoneNumber.CountryCode;
            }
            catch (System.Exception)
            {
                // return default country code if number not found
                return "US";
            }
        }


        private string ConvertKelvinToCelcius(string kelvin)
        {
            var tempInKelvin = Convert.ToDouble(kelvin);
            return (tempInKelvin - 273.15).ToString();
        }

        private string GetPrice(string messagetext, string countryId)
        {
            var reply = $"Commodity price not found. Try {CropExtensions.PrintAllCrops()}";
            if (string.IsNullOrEmpty(messagetext))
            {
                return reply;
            }

            var list = messagetext.Split(":");
            if (list.Length > 1)
            {
                var cropName = list[1].Replace("[", "").Replace("]", "").Trim();
                CropInfo info = infoReader.GetCropInfo(cropName, countryId);
                reply = info.toString();
            }
            
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
