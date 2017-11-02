import requests
import os

# Get Weather Data using Open Weather API

api_key = 'http://api.openweathermap.org/data/2.5/weather?appid=c38e770a1da2d9c102a1e4a5ae98f0d1&q='
city = input("Input your city name: ")
url = api_key + city


data = requests.get(url).json()

formatted_data = data['main']['temp']



print(str(formatted_data) + " Kelvin")