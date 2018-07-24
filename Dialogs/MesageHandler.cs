using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using SimpleEchoBot.FarmingInfo;

namespace HelpFarmerViaSMS
{
    [Serializable]
    public class MessageHandler : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            //await context.PostAsync($"{this.count++}: repeating: {message.Text}");
            //context.Wait(MessageReceivedAsync);
            var message = await argument;
            CropInfoReader infoReader = new CropInfoReader();

            var crop = (Crop)Int32.Parse(message.Text);
            CropInfo info = infoReader.GetCropInfo(crop);
            var logMessage = $"{info.name} low price: {info.lowPrice} avg price: {info.avgPrice} high price: {info.highPrice}"; 
            await context.PostAsync(logMessage);
            context.Wait(MessageReceivedAsync);
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            context.Wait(MessageReceivedAsync);
        }

    }
}