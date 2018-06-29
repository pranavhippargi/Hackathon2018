# -*- coding: utf-8 -*-

import http.client, urllib.parse, uuid, json

class DetectLanguage():
        
        # Replace the subscriptionKey string value with your valid subscription key.
    subscriptionKey = '980ff8a9930940afb0e6465965d74832'
    host = 'api.cognitive.microsofttranslator.com'
    path = '/translate?api-version=3.0'


    host = 'api.cognitive.microsofttranslator.com'
    path = '/breaksentence?api-version=3.0'

    params = ''

    #Let's make this text dynamic. 
    text = 'bonjour, je suis Pranav'

    def breakSentences (content):
        host = 'api.cognitive.microsofttranslator.com'
        path = '/translate?api-version=3.0'
        headers = {

            'Ocp-Apim-Subscription-Key': '980ff8a9930940afb0e6465965d74832',
            'Content-type': 'application/json',
            'X-ClientTraceId': str(uuid.uuid4())
                
        }

        conn = http.client.HTTPSConnection(host)
        conn.request ("POST", path + params, content, headers)
        response = conn.getresponse ()
        return response.read ()

        requestBody = [{
            'Text' : text,
            }]
        content = json.dumps(requestBody, ensure_ascii=False).encode('utf-8')
        result = breakSentences (content)

    # Note: We convert result, which is JSON, to and from an object so we can pretty-print it.
    # We want to avoid escaping any Unicode characters that result contains. See:
    # https://stackoverflow.com/questions/18337407/saving-utf-8-texts-in-json-dumps-as-utf8-not-as-u-escape-sequence
        output = json.dumps(json.loads(result), indent=4, ensure_ascii=False)
        print(output) 
        

q = DetectLanguage
q.breakSentences("Hola")


