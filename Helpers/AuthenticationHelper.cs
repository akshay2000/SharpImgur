using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace SharpImgur.Helpers
{
    public static class AuthenticationHelper
    {
        private const string authUrlPattern = "https://api.imgur.com/oauth2/authorize?response_type=token&client_id={0}&state=yo";
        private const string callback = "http://localhost:8080/MonocleGiraffe";

        private static Dictionary<string, string> authResult;
        private const string userNameKey = "account_username";
        private const string accessTokenKey = "access_token";
        private const string refreshTokenKey = "refresh_token";
        private const string isAuthIntendedKey = "IsAuthIntended";

        public static async Task<string> Auth()
        {
            SettingsHelper.SetLocalValue(isAuthIntendedKey, false);
            JObject config = await SecretsHelper.GetConfiguration();
            Uri uri = new Uri(string.Format(authUrlPattern, config["Client_Id"]));
            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, uri, new Uri(callback));
            SettingsHelper.SetLocalValue(isAuthIntendedKey, true);
            return result.ResponseData;
        }

        public static Dictionary<string, string> ParseAuthResult(string result)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Uri uri = new Uri(result.Replace('#', '&'));
            string query = uri.Query;
            string[] frags = query.Split('&');
            foreach (var frag in frags)
            {
                string[] splits = frag.Split('=');
                ret.Add(splits[0], splits[1]);
            }
            return ret;                
        }

        private static async Task<Dictionary<string, string>> GetAuthResult()
        {
            if (authResult == null)
            {
                string resultString = await Auth();
                authResult = ParseAuthResult(resultString);
            }
            return authResult;
        }

        public static async Task<string> GetAccessToken()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[accessTokenKey];
        }

        public static async Task<string> GetRefreshToken()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[refreshTokenKey];
        }

        public static async Task<string> GetUserName()
        {
            Dictionary<string, string> result = await GetAuthResult();
            return result[userNameKey];
        }

        public static bool IsAuthIntended()
        {
            return SettingsHelper.GetLocalValue<bool>(isAuthIntendedKey, false);
        }
    }
}
