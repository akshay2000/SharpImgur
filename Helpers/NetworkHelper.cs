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
        #region Imgur

        private static HttpClient httpClient;
        private static HttpClient authHttpClient;
        private static string baseURI = "https://imgur-apiv3.p.mashape.com/3/";
        private static string imgurBaseURI = "https://api.imgur.com/3/";

        public static async Task<JObject> ExecuteRequest(string url, bool isNative = false)
        {
#if DEBUG
            isNative = true;
#endif
            string finalUrl;
            if (url.StartsWith("http"))
                finalUrl = url;
            else {
                finalUrl = isNative ? imgurBaseURI + url : baseURI + url;
            }
            HttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();
            string response = "{}";
            try
            {
                var r = await httpClient.GetAsync(new Uri(finalUrl));
                response = await r.Content.ReadAsStringAsync();
            }
            catch
            {
                Debug.WriteLine("Netwrok Error!");
            }
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        public static async Task<JObject> ExecutePostRequest(string relativeUri, JObject payload, bool isNative = false)
        {
            string uri = isNative ? imgurBaseURI + relativeUri : baseURI + relativeUri;
            return await ExecutePostRequest(new Uri(uri), payload, isNative);
        }

        public static async Task<JObject> ExecutePostRequest(Uri uri, JObject payload, bool isNative = false)
        {
            HttpClient httpClient = AuthenticationHelper.IsAuthIntended() ? await GetAuthClient() : await GetClient();            
            string response = "{}";
            try
            {
                IHttpContent content = new HttpStringContent(payload.ToString(), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                var r = await httpClient.PostAsync(uri, content);
                response = await r.Content.ReadAsStringAsync();
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

        #endregion

        #region Reddit

        public static async Task<JObject> ExecuteRedditRequest(string url)
        {
            HttpClient httpClient = GetRedditClient();
            string response = "{}";
            try
            {
                response = await httpClient.GetStringAsync(new Uri(url));
            }
            catch
            {
                Debug.WriteLine("Netwrok Error!");
            }
            JObject responseJson = JObject.Parse(response);
            return responseJson;
        }

        private static HttpClient redditClient;
        private static HttpClient GetRedditClient()
        {
            if (redditClient == null)
            {
                redditClient = new HttpClient();
            }
            return redditClient;
        }

        #endregion
    }
}
