using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SharpImgur.Helpers
{
    public class NetworkHelper
    {
        private static HttpClient httpClient;
        private static string baseURI = "https://imgur-apiv3.p.mashape.com/3/";
        private static JObject configuration;

        public static async Task<string> ExecuteRequest(string relativeUri)
        {
            HttpClient httpClient = await GetClient();
            string uri = baseURI + relativeUri;
            string response = await httpClient.GetStringAsync(new Uri(uri));
            JObject responseJson = JObject.Parse(response);
            return responseJson.ToString();
        }

        private static async Task<HttpClient> GetClient()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
                JObject config = await GetConfiguration();
                httpClient.DefaultRequestHeaders["Authorization"] = "Client-ID " + (string)config["Client_Id"];
                httpClient.DefaultRequestHeaders["X-Mashape-Key"] = (string)config["Mashape_Key"];
            }
            return httpClient;
        }

        private static async Task<JObject> GetConfiguration()
        {
            if (configuration == null)
            {
                var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var libFolder = await installationFolder.GetFolderAsync("SharpImgur");
                var file = await libFolder.GetFileAsync("Secrets.json");
                string configurationString = await Windows.Storage.FileIO.ReadTextAsync(file);
                configuration = JObject.Parse(configurationString);
            }
            return configuration;
        }
    }
}
