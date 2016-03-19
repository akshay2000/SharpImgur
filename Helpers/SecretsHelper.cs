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
            const string resource = "AccessToken";
            string token;
            try
            {
                DateTime expiry = DateTime.Parse(SettingsHelper.GetLocalValue<string>(expiryKey, DateTime.MinValue.ToString()));
                if (expiry == DateTime.MinValue)
                    throw new Exception("Force execute catch block");
#if DEBUG
                expiry = DateTime.MinValue;
#endif
                if (expiry > DateTime.Now)
                {
                    token = GetVault().Retrieve(resource, userName).Password;
                }
                else
                {
                    token = await AuthenticationHelper.RefreshAccessToken(await GetRefreshToken());
                    SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
                }
            }
            catch
            {
                token = await AuthenticationHelper.GetAccessToken();
                PasswordCredential cred = new PasswordCredential(resource, userName, token);
                GetVault().Add(cred);
                SettingsHelper.SetLocalValue(expiryKey, (await AuthenticationHelper.GetExpiresAt()).ToString());
            }
            return token;
        }

        public static async Task<string> GetRefreshToken()
        {
            string userName = await GetUserName();
            const string resource = "RefreshToken";
            string token;
            try
            {
                token = GetVault().Retrieve(resource, userName).Password;
            }
            catch
            {
                token = await AuthenticationHelper.GetRefreshToken();
                PasswordCredential cred = new PasswordCredential(resource, userName, token);
                GetVault().Add(cred);
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

        public static async Task RefreshSecrets()
        {
            string userName = await AuthenticationHelper.GetUserName();
            SettingsHelper.SetLocalValue(userNameKey, userName);
            string accessToken = await AuthenticationHelper.GetAccessToken();
            PasswordCredential aCred = new PasswordCredential("AccessToken", userName, accessToken);
            GetVault().Add(aCred);
            string refreshToken = await AuthenticationHelper.GetRefreshToken();
            PasswordCredential rCred = new PasswordCredential("RefreshToken", userName, refreshToken);
            GetVault().Add(rCred);
        }
    }
}
