using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SharpImgur.Helpers
{
    public class SettingsHelper
    {
        private static ApplicationDataContainer AppSettings = ApplicationData.Current.RoamingSettings;
        private static ApplicationDataContainer AppLocalSettings = ApplicationData.Current.LocalSettings;

        public static void SetValue(string key, object value)
        {
            AppSettings.Values[key] = value;
        }

        public static T GetValue<T>(string key)
        {
            return (T)AppSettings.Values[key];
        }

        public static T GetValue<T>(string key, object defaultValue)
        {
            if (AppSettings.Values[key] == null)
            {
                AppSettings.Values[key] = defaultValue;
            }
            return (T)AppSettings.Values[key];
        }

        public static void SetLocalValue(string key, object value)
        {
            AppLocalSettings.Values[key] = value;
        }

        public static T GetLocalValue<T>(string key)
        {
            return (T)AppLocalSettings.Values[key];
        }

        public static T GetLocalValue<T>(string key, object defaultValue)
        {
            if (AppLocalSettings.Values[key] == null)
            {
                AppLocalSettings.Values[key] = defaultValue;
            }
            return (T)AppLocalSettings.Values[key];
        }

        public static void RemoveLocalValue(string key)
        {
            AppLocalSettings.Values.Remove(key);
        }
    }
}
