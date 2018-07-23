# Web scraper
import requests
# OS Envirnoment Variables so that the world doesn't steal our keys
# But seriously. 
import os


# Class for Getting Weather
class Weather():
    
    # No instance variables needed. 
    def __init__(self):
        pass
        
    # Get Weather Data using Open Weather API
    def getWeather(self):
        
        # API Key should be in an OS Environment Variable.
        api_key = 'http://api.openweathermap.org/data/2.5/weather?appid=c38e770a1da2d9c102a1e4a5ae98f0d1&q='
       
       # For now, it's asking to get the City through the command line. 
       # We can programmatically make it go into the text voice conversation
              
        city = input("Input your city name: ")
        
        # Url will append the City Name to the end of the key and return JSON
        url = api_key + city

        # Get the weather Data in an environment named data
        data = requests.get(url).json()

        # Get the most relevant information
        # Here, it's just the temperature 
        # Temperature will return Kelvin. 
        # weather_celcius converts Kelvin to Celcius
        # Returns results in two decimal places. 
        #Test
        weather_kelvin = data['main']['temp']
        weather_celcius = weather_kelvin - 237.15
        print(weather_kelvin)
        return ("%.2f" % weather_celcius)


    
    
