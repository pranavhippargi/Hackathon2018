import requests
import os
from twilio.rest import Client


# Get Weather Data using Open Weather API
class Weather():
    
    def __init__(self):
        pass
        
    def getWeather(self):
        api_key = 'http://api.openweathermap.org/data/2.5/weather?appid=c38e770a1da2d9c102a1e4a5ae98f0d1&q='
        city = input("Input your city name: ")
        url = api_key + city
        data = requests.get(url).json()
        weather_kelvin = data['main']['temp']
        weather_celcius = weather_kelvin - 237.15
        return ("%.2f" % weather_celcius)
        
# class Text(Weather):   

#     def __init__(self):
#         pass
        
#     def sendText(self): 
#         y = Weather() 
#         y.getWeather()
#         message = "It is " + str(y.getWeather()) + " today"
        



    
