using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace HelpFarmerViaSMS
{
    [Serializable]
    public class MessageHandler : IDialog<object>
    {
        private CropInfoReader infoReader;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            //await context.PostAsync($"{this.count++}: repeating: {message.Text}");
            //context.Wait(MessageReceivedAsync);
            var crop = (Crop)Int32.Parse(message.Text);
            CropInfo info = infoReader.GetCropInfo(crop);
            var message = $"{info.name} low price: {info.lowPrice} avg price: {info.avgPrice} high price: {info.highPrice}"; 
            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}