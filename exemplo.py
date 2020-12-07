import hashlib
import hmac
import json
import time

from http import client
from urllib.parse import urlencode

# Constantes
MB_TAPI_ID = '<chave_tapi>'
MB_TAPI_SECRET = '<segredo>'
REQUEST_HOST = 'www.mercadobitcoin.net'
REQUEST_PATH = '/tapi/v3/'

# Nonce
tapi_nonce = str(int(time.time()))

# Parâmetros
params = {
    'tapi_method': 'get_account_info',
    'tapi_nonce': tapi_nonce,
}
params = urlencode(params)

# Gerar MAC
params_string = REQUEST_PATH + '?' + params
H = hmac.new(bytes(MB_TAPI_SECRET, encoding='utf8'), digestmod=hashlib.sha512)
H.update(params_string.encode('utf-8'))
tapi_mac = H.hexdigest()

# Gerar cabeçalho da requisição
headers = {
    'Content-Type': 'application/x-www-form-urlencoded',
    'TAPI-ID': MB_TAPI_ID,
    'TAPI-MAC': tapi_mac
}

# Realizar requisição POST
try:
    conn = client.HTTPSConnection(REQUEST_HOST)
    conn.request("POST", REQUEST_PATH, params, headers)

    # Exibindo dados da resposta no console
    response = conn.getresponse()
    response = response.read()

    response_json = json.loads(response)
    print('status: {}'.format(response_json['status_code']))
    print(json.dumps(response_json, indent=4))
finally:
    if conn:
        conn.close()