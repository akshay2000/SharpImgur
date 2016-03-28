using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace SharpImgur.Helpers
{
    public class SecretsHelper
    {
        private static JObject configuration;
        private static PasswordVault vault;

        private const string userNameKey = "UserName";
        private const string expiryKey = "Expiry";

        private const string accessResource = "AccessToken";
        private const string refreshResource = "RefreshToken";

        public static async Task<JObject> GetConfiguration()
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

        public static async Task<string> GetAccessToken()
        {
            string userName = await GetUserName();
            string token;
            DateTime expiry = DateTime.Parse(SettingsHelper.GetLocalValue<string>(expiryKey, DateTime.MinValue.ToString()));
            if (expiry == DateTime.MinValue)
            {
                token = await AuthenticationHelper.GetAccessToken();
                UpdateCredentials(token, await AuthenticationHelper.GetRefreshToken());
                SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
            }
            else if (expiry > DateTime.Now)
            {
                token = GetVault().Retrieve(accessResource, userName).Password;
            }
            else
            {
                token = await AuthenticationHelper.RefreshAccessToken(await GetRefreshToken());
                UpdateCredentials(token, await AuthenticationHelper.GetRefreshToken());
                SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
            }
            return token;
        }

        public static async Task<string> GetRefreshToken()
        {
            string userName = await GetUserName();
            string token;
            var currentVault = GetVault();
            if (currentVault.Contains(refreshResource, userName))
            {
                token = GetVault().Retrieve(refreshResource, userName).Password;
            }
            else
            {
                token = await AuthenticationHelper.GetRefreshToken();
                UpdateCredentials(await AuthenticationHelper.GetAccessToken(), token);
            }
            return token;
        }

        private static PasswordVault GetVault()
        {
            if (vault == null)
                vault = new PasswordVault();
            return vault;
        }

        private static string userName;
        public static async Task<string> GetUserName()
        {
            if (userName == null)
                userName = SettingsHelper.GetLocalValue<string>(userNameKey);
            if (userName == null)
            {
                userName = await AuthenticationHelper.GetUserName();
                SettingsHelper.SetLocalValue(userNameKey, userName);
            }
            return userName;
        }

        private static void UpdateCredentials(string accessToken = null, string refreshToken = null)
        {
            if (accessToken != null)
            {
                PasswordCredential aCred = new PasswordCredential(accessResource, userName, accessToken);
                GetVault().Add(aCred);
            }
            if (refreshToken != null)
            {
                PasswordCredential rCred = new PasswordCredential(refreshResource, userName, refreshToken);
                GetVault().Add(rCred);
            }
        }

        public static async Task RefreshSecrets()
        {
            string userName = await AuthenticationHelper.GetUserName();
            SettingsHelper.SetLocalValue(userNameKey, userName);
            string accessToken = await AuthenticationHelper.GetAccessToken();
            PasswordCredential aCred = new PasswordCredential(accessResource, userName, accessToken);
            GetVault().Add(aCred);
            string refreshToken = await AuthenticationHelper.GetRefreshToken();
            PasswordCredential rCred = new PasswordCredential(refreshResource, userName, refreshToken);
            GetVault().Add(rCred);
        }
    }
}
