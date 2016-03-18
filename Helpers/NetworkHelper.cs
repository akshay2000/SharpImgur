using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SharpImgur.Helpers
{
    public class NetworkHelper
    {
        private static HttpClient httpClient;
        private static HttpClient authHttpClient;
        private static string baseURI = "https://imgur-apiv3.p.mashape.com/3/";
        private static string imgurBaseURI = "https://api.imgur.com/3/";

        public static async Task<JObject> ExecuteRequest(string relativeUri, bool isNative = false)
        {
            HttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();
            string uri = isNative ? imgurBaseURI + relativeUri : baseURI + relativeUri;
            string response = "{}";
            try
            {
                response = await httpClient.GetStringAsync(new Uri(uri));
            }
            catch
            {
                Debug.WriteLine("Netwrok Error!");
            }
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        private static async Task<HttpClient> GetClient()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();                
                JObject config = await SecretsHelper.GetConfiguration();
                httpClient.DefaultRequestHeaders["Authorization"] = "Client-ID " + (string)config["Client_Id"];
                httpClient.DefaultRequestHeaders["X-Mashape-Key"] = (string)config["Mashape_Key"];               
            }
            return httpClient;
        }        
        
        private static async Task<HttpClient> GetAuthClient()
        {
            if (authHttpClient == null)
            {
                authHttpClient = new HttpClient();
                JObject config = await SecretsHelper.GetConfiguration();
                authHttpClient.DefaultRequestHeaders["X-Mashape-Key"] = (string)config["Mashape_Key"];
                authHttpClient.DefaultRequestHeaders["Authorization"] = "Bearer " + await SecretsHelper.GetAccessToken();
            }
            return authHttpClient;
        }        
    }
}
