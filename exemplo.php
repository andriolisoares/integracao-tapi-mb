<?php

    // Constantes
    define("TAPI_ID", "<chave_tapi>");
    define("TAPI_SECRET", "<segredo>");
    define("ENDPOINT_TRADE_PATH", "/tapi/v3/");
    define("ENDPOINT_TRADE_API", "https://www.mercadobitcoin.net" . ENDPOINT_TRADE_PATH);

    // Nonce
    $date = new DateTime();
    $tapi_nonce = $date->getTimestamp();

    // Parâmetros
    $params = array(
        'tapi_method' => 'get_account_info',
        'tapi_nonce' => $tapi_nonce
    );
    $params = http_build_query($params);

    // Gerar MAC
    $params_string = ENDPOINT_TRADE_PATH . '?' . $params;
    $tapi_mac = hash_hmac('sha512', $params_string, TAPI_SECRET);

    # Gerar cabeçalho da requisição
    $headers = array(
        'Content-Type: application/x-www-form-urlencoded',
        'TAPI-ID: '. TAPI_ID,
        'TAPI-MAC: '. $tapi_mac
    );

    // Realizar requisição POST
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, ENDPOINT_TRADE_API);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true); 
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
    curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
    curl_setopt($ch, CURLOPT_POSTFIELDS, $params);
    curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);

    $response = curl_exec($ch);
    $json_response = json_decode($response);
    $info = curl_getinfo($ch);
    curl_close($ch);

    // Exibindo dados da resposta no console
    print_r($json_response);
?>