import requests
import json

url = 'http://localhost:3000/api/login'
data = {
    'username': 'Omer',
    'password': '5'
}
headers = {
    'Content-Type': 'application/json'
}

response = requests.post(url, json=data, headers=headers)
print('Status:', response.status_code)
print('Response:', response.text)