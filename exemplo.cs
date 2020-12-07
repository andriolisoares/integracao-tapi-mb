ServicePointManager.Expect100Continue = true;
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

using System;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using System.Security.Cryptography;

class Chamada 
{
    public static void Main(string[] args) 
    {   
        // Constantes
        string mbTapiID = "<chave_tapi>";
	    string mbTapiSecret = "<segredo>";
	    string requestHost = "https://www.mercadobitcoin.net";
	    string requestPath = "/tapi/v3/";

        // Nonce
        int nonce = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        // Gerar MAC
        string message = "/tapi/v3/?tapi_method=get_account_info&tapi_nonce=" + nonce.ToString();
        byte[] postData = Encoding.UTF8.GetBytes(message);
        byte[] ms = Encoding.UTF8.GetBytes(mbTapiSecret);
        byte[] hash = new HMACSHA512(ms).ComputeHash(postData);
        string mac = BitConverter.ToString(hash).Replace("-", "").ToLower();

        // Parâmetros
        string paramsEncoded = "tapi_method=" + Uri.EscapeDataString("get_account_info");
            paramsEncoded += "&tapi_nonce=" + Uri.EscapeDataString(nonce.ToString());
        byte[] data = Encoding.UTF8.GetBytes(paramsEncoded);
        
        // Realizar requisição POST
        var request = (HttpWebRequest)WebRequest.Create(requestHost+requestPath); 
        request.Method = "POST";

        // Gerar cabeçalho da requisição
        request.Headers.Add("TAPI-ID", mbTapiID);
        request.Headers.Add("TAPI-MAC", mac);
        request.ContentType = "application/x-www-form-urlencoded";      
        request.ContentLength = data.Length;

        // Obter o stream com o conteúdo da requisição
        Stream stream = request.GetRequestStream();
        stream.Write(data, 0, data.Length);
        stream.Close();

        // Obter resposta
        var response = (HttpWebResponse)request.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        
        // Exibindo dados da resposta no console
        Console.WriteLine(responseString);

        // Fechando a resposta
        response.Close();
    }
}
Chamada.Main(null);