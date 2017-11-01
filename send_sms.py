from twilio.rest import Client
from credentials import account_sid, auth_token, my_cell, my_twilio

client = TwilioRestClient('ACdf3fe78024277d62b37d232d6c420592', '125903a4cc62889e863b0f775caed4d7')

my_msg = "Your message goes here..."

message = client.messages.create(to="+19163379294", from_="5303631182",
                                     body=my_msg)
