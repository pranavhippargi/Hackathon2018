from flask import Flask, Response, request
from twilio.twiml.messaging_response import Message, MessagingResponse


app = Flask(__name__)


@app.route("/twilio")
def check_app():
    # returns a simple string stating the app is working
    return Response("It works!"), 200


@app.route("/twilio", methods=["POST"])
def inbound_sms():
    response = MessagingResponse()
    # we get the SMS message from the request. we could also get the 
    # "To" and the "From" phone number as well
    inbound_message = request.form.get("Body")
    
    # we can now use the incoming message text in our Python application
    if inbound_message == "Hello":
        response.message("Hello back to you! Respond 1 for Weather, 2 for Crop Price, and 3 for inventory")
    elif inbound_message == "1":
        response.message("The Weather is in Seattle")
    else:
        response.message("Hi! Not quite sure what you meant, but okay.")
    
    # we return back the mimetype because Twilio needs an XML response
    return Response(str(response), mimetype="application/xml"), 200


if __name__ == "__main__":
    app.run(debug=True)