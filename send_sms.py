from twilio.rest import Client
from weatherdata import Weather

class Main(Weather):
        
    def send_sms(self):
        x = Weather()
        messageText = "It is " + str(x.getWeather()) + " Celcius, today"
        account_sid = 'AC1061e132398b5dc01334d9ff9b63d58a'
        auth_token = '9e6a04f6a51ef7d32858ac16f83c2727'
        client = Client(account_sid, auth_token)
        message = client.messages.create(to="+19163379294", from_="8459996189",
                                        body=messageText)
x= Main()
x.send_sms()
