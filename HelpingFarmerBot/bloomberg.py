import requests
import json
from bs4 import BeautifulSoup

headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36'}
page = requests.get('https://www.bloomberg.com/markets/commodities/futures/agriculture', headers=headers)
soup = BeautifulSoup(page.text, 'html.parser')
script = soup.findAll('script')
block = ""
for js in script:
	if js.find("bootstrappedData") != -1:
		block = str(js)


data = block.split('\n')[5]
data = data.replace("bootstrappedData: ","")
print(data)
'''
p = []
for line in block.splitlines():
	l = line.strip()
	if not l:continue
	p.append(l.split(":"))

bootstrappedData = p[5]
if bootstrappedData[0] == 'bootstrappedData':
	del bootstrappedData[0]

s = ''.join(bootstrappedData)
print(s)
#j = json.loads(s)
#print(j)
'''