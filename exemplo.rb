require 'rubygems' if RUBY_VERSION < '1.9'
require 'rest_client'

# Constantes
mbTapiID = '<chave_tapi>'
mbTapiSecret = '<segredo>'
requestHost = 'https://www.mercadobitcoin.net'
requestPath = '/tapi/v3/'

# Nonce
tapi_nonce = Time.now.to_i

# Parâmetros
params = 'tapi_method=get_account_info&tapi_nonce=' + tapi_nonce.to_s

# Gerar MAC
params_string = requestPath + '?' + params
tapi_mac = OpenSSL::HMAC.hexdigest('sha512', mbTapiSecret, params_string)

# Gerar cabeçalho da requisição
headers = {
  :'Content-Type' => 'application/x-www-form-urlencoded',
  :'TAPI-ID' => mbTapiID,
  :'TAPI-MAC' => tapi_mac,
}

# Realizar requisição POST
response = RestClient.post(requestHost+requestPath, params, headers)

# Exibindo dados da resposta no console
print response
