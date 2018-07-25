using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using SimpleEchoBot.FarmingInfo;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelpingFarmerBot
{
    public class EchoBot : IBot
    {
        /// <summary>
        /// Every Conversation turn for our EchoBot will call this method. In here
        /// the bot checks the Activty type to verify it's a message, bumps the 
        /// turn conversation 'Turn' count, and then echoes the users typing
        /// back to them. 
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurn(ITurnContext context)
        {
            //// This bot is only handling Messages
            //if (context.Activity.Type == ActivityTypes.Message)
            //{
            //    //// Get the conversation state from the turn context
            //    //var state = context.GetConversationState<EchoState>();

            //    //// Bump the turn count. 
            //    //state.TurnCount++;

            //    //// Echo back to the user whatever they typed.
            //    //await context.SendActivity($"Turn {state.TurnCount}: TanuK sent '{context.Activity.Text}'");


            //}


            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:
                    var message = context.Activity.Text.Trim().ToLower();

                    // echo back the user's input.
                    
                    if (message.ToLowerInvariant().Contains("weather"))
                    {
                        try
                        {
                            var apiKey = "d0350e61d7e88eac4874c96c578cb95b";
                            var newUrl = $"http://api.openweathermap.org/data/2.5/weather?q=London,uk&APPID={apiKey}";

                            var weatherJsonMsg = this.HttpGet(newUrl);
                            var weatherData = JsonConvert.DeserializeObject<WeatherData>(weatherJsonMsg);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    
                    CropInfoReader infoReader = new CropInfoReader();

                    var crop = (Crop)Int32.Parse(message);
                    CropInfo info = infoReader.GetCropInfo(crop);
                    var logMessage = $"{info.name} low price: {info.lowPrice} avg price: {info.avgPrice} high price: {info.highPrice}";
                    await context.SendActivity(logMessage);
                    break;



                case ActivityTypes.ConversationUpdate:
                    foreach (var newMember in context.Activity.MembersAdded)
                    {
                        if (newMember.Id != context.Activity.Recipient.Id)
                        {
                            await context.SendActivity("Hello and welcome to the echo bot.");
                            await context.SendActivity("Hello, you are choosing to use the Microsoft Translator API Bot.");
                        }
                    }
                    break;
            }

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
