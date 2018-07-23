
# Twilio Imports
from twilio.rest import Client
# Weather class
from weatherdata import Weather

# Main Scripts
class Main(Weather):
    
    def __init__(self):
        pass

    #Send SMS
    # Get weather from Weather Class
    # Should not put out keys here. oops. 
    # send message to and from.    
    def send_sms(self):
        x = Weather()
        messageText = "It is " + str(x.getWeather()) + " Celcius, today"
        account_sid = 'AC1061e132398b5dc01334d9ff9b63d58a'
        auth_token = '9e6a04f6a51ef7d32858ac16f83c2727'
        client = Client(account_sid, auth_token)

        # We should make these numbers (to, from) dynamic as well as the message_Text
        message = client.messages.create(to="+19163379294", from_="8459996189",
                                        body=messageText)

        return(self.message.body)
    #Get Message Body and send to Microsoft Translator
    def sendTextBodyToMSFTTranslate(self):
        print(message.body)
        

# OFF WE GO! 
x= Main()
x.send_sms()
