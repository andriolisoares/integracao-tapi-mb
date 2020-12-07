const https = require('https');
const crypto = require('crypto');
const querystring = require('querystring');

// Constantes
const MB_TAPI_ID = '<chave_tapi>';
const MB_TAPI_SECRET = '<segredo>';
const REQUEST_HOST = 'www.mercadobitcoin.net';
const REQUEST_PATH = '/tapi/v3/';

// Nonce
let tapi_nonce = Math.floor(Date.now() / 1000);

// Parâmetros
let params = {
    'tapi_method': 'get_account_info',
    'tapi_nonce': tapi_nonce
};
params = querystring.stringify(params);

// Gerar MAC
let params_string = REQUEST_PATH + '?' + params;
let hash = crypto.createHmac('sha512', MB_TAPI_SECRET);
hash.update(params_string);
let tapi_mac = hash.digest('hex');

// Gerar cabeçalho da requisição
let headers = {
    'Content-Type': 'application/x-www-form-urlencoded',
    'TAPI-ID': MB_TAPI_ID,
    'TAPI-MAC': tapi_mac
};

// Parâmetros da requisição POST
const options = {
    hostname: REQUEST_HOST,
    path: REQUEST_PATH,
    method: 'POST',
    headers: headers
};
// Realizar requisição POST
const req = https.request(options, (res) => {
    let data = '';
    res.on('data', (chunk) => {
        data += chunk;
    });

    res.on('end', () => {
        // Exibindo resposta no console
        console.log(JSON.parse(data));
    })

}).on('error', (err) => {
    console.log("Error: " + err.message)
});
// Passando parâmetros
req.write(params)
req.end();