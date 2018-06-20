import requests
import os
from twilio.rest import Client


# Get Weather Data using Open Weather API
class weather():
    def getWeather(self):
        api_key = 'http://api.openweathermap.org/data/2.5/weather?appid=c38e770a1da2d9c102a1e4a5ae98f0d1&q='
        city = input("Input your city name: ")
        url = api_key + city
        data = requests.get(url).json()
        formatted_data = data['main']['temp']
        return formatted_data
        
        
        
    def sendText(self):  
        y = weather()
        message = "It is " + str(y.getWeather()) + " today"
        print(str(y) + " Kelvin")
        account_sid = 'AC1061e132398b5dc01334d9ff9b63d58a'
        auth_token = '9e6a04f6a51ef7d32858ac16f83c2727'
        client = Client(account_sid, auth_token)
        message = client.messages.create(to="+19163379294", from_="8459996189",
                                        body=message)

x = weather()
# x.getWeather()
x.sendText()



    
