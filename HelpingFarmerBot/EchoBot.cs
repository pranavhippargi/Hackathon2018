using System;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using SimpleEchoBot.FarmingInfo;

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
                    var reply = GetCropMessage(message);
                    await context.SendActivity(reply);
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

        private string GetCropMessage(string input)
        {
            CropInfoReader infoReader = new CropInfoReader();

            var crop = (Crop)Int32.Parse(input);
            CropInfo info = infoReader.GetCropInfo(crop, "");
            return $"{info.name} low price: {info.lowPrice} avg price: {info.avgPrice} high price: {info.highPrice}";
        }
    }
}
