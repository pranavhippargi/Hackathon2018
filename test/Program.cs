using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Lookups.V1;





class Program {
    static void Main(string[] args)
    {
        // Find your Account Sid and Token at twilio.com/console
        const string accountSid = "AC1061e132398b5dc01334d9ff9b63d58a";
        const string authToken = "9e6a04f6a51ef7d32858ac16f83c2727";

        TwilioClient.Init(accountSid, authToken);

        var type = new List<string> {
            "carrier"
        };


        var phoneNumber = PhoneNumberResource.Fetch(
            type: type,
            pathPhoneNumber: new Twilio.Types.PhoneNumber("+61452627666")

        );

        Console.WriteLine(phoneNumber.CountryCode);
        
    }
}
