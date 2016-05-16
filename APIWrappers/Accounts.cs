using Newtonsoft.Json.Linq;
using SharpImgur.Helpers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImgur.APIWrappers
{
    public static class Accounts
    {
        public static async Task<GalleryProfile> GetGalleryProfile(string userName)
        {
            const string urlPattern = "account/{0}/gallery_profile";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result["data"].ToObject<GalleryProfile>();
        }

        public static async Task<Account> GetAccount(string userName)
        {
            const string urlPattern = "account/{0}";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result["data"].ToObject<Account>();
        }

        public static async Task<List<Image>> GetImages(string userName)
        {
            const string urlPattern = "account/{0}/images/";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result["data"].ToObject<List<Image>>();
            //return new List<Image>();
        }

        private static async Task<int> GetImageCount(string userName)
        {
            const string urlPattern = "account/{0}/images/count";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result["data"].ToObject<int>();
        }

        public static async Task<List<Album>> GetAlbums(string userName)
        {
            const string urlPattern = "account/{0}/albums/";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result["data"].ToObject<List<Album>>();
        }        

        private static async Task<int> GetAlbumCount(string userName)
        {
            const string urlPattern = "account/{0}/albums/count";
            string url = string.Format(urlPattern, userName);
            JObject result = await NetworkHelper.ExecuteRequest(url);
            return result["data"].ToObject<int>();
        }
    }
}
