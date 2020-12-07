package main

import (
	"crypto/hmac"
	"crypto/sha512"
	"encoding/hex"
	"fmt"
	"io/ioutil"
	"net/http"
	"net/url"
	"strconv"
	"strings"
	"time"
)

func main() {

	// Constantes
	mbTapiID := "<chave_tapi>"
	mbTapiSecret := "<segredo>"
	requestHost := "https://www.mercadobitcoin.net"
	requestPath := "/tapi/v3/"

	// Nonce
	tapiNonce := strconv.FormatInt(time.Now().Unix(), 10)
	fmt.Println("Nonce:", tapiNonce)

	// Parâmetros
	params := url.Values{}
	params.Add("tapi_method", "get_account_info")
	params.Add("tapi_nonce", tapiNonce)

	// Gerar MAC
	paramsString := requestPath + "?" + params.Encode()
	h := hmac.New(sha512.New, []byte(mbTapiSecret))
	h.Write([]byte(paramsString))
	tapiMac := hex.EncodeToString(h.Sum(nil))

	// Realizar requisição POST
	request, err := http.NewRequest("POST", requestHost+requestPath, strings.NewReader(params.Encode()))

	// Gerar cabeçalho da requisição
	request.Header.Set("TAPI-ID", mbTapiID)
	request.Header.Set("TAPI-MAC", tapiMac)
	request.Header.Set("Content-Type", "application/x-www-form-urlencoded")

	client := &http.Client{}
	resp, err := client.Do(request)
	if err != nil {
		panic(err)
	}
	defer resp.Body.Close()

	// Exibindo dados da resposta no console
	fmt.Println("response Status:", resp.Status)
	body, _ := ioutil.ReadAll(resp.Body)
	fmt.Println("response Body:", string(body))
}
