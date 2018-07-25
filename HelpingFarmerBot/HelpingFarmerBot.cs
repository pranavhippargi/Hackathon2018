﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace HelpingFarmerBot
{
    public class HelpingFarmerBot : IBot
    {
        private static string HelpMessage = $"Welcome to Infarmation. \nDiscover global crop prices and local weather forecasts. Try following options. \n Price: 'Crop' \n Weather: 'City'";
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
                    var country = context.Activity.From.Id;
                    await context.SendActivity(country);

                    if (messagetext.Contains("price"))
                    {
                        var message = GetPrice(messagetext);
                        await context.SendActivity(message);
                    }
                    else if(messagetext.Contains("weather"))
                    {
                        await context.SendActivity($"Tomorrow weather in London is rainy.");
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

        private string GetPrice(string messagetext)
        {
            var defaultMessage = "Commodity price not found. Try - Price: 'Wheat'.";
            if (string.IsNullOrEmpty(messagetext))
            {
                return defaultMessage;
            }

            var list = messagetext.Split(":");
            var cropName = list[1].Replace("[","").Replace("]","").Trim();
            CropInfo info = infoReader.GetCropInfo(cropName, "");
            var reply = $"{info.name} price today - low: {info.lowPrice} average: {info.avgPrice} high: {info.highPrice}";

            return reply;

        }
    }
}
