using Newtonsoft.Json.Linq;
using SharpImgur.Helpers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpImgur.APIWrappers.Enums;

namespace SharpImgur.APIWrappers
{
    public static class Gallery
    {
        public static async Task<List<Image>> GetGallery(Section? section = null, Sort? sort = null, Window? window = null, bool? showViral = null, int? page = null)
        {
            string uri = "gallery";
            if (section != null)
            {
                uri += "/" + section.ToString().ToLower();
                if(sort != null)
                {
                    uri += "/" + sort.ToString().ToLower();
                    if(window != null)
                    {
                        uri += "/" + window.ToString().ToLower();
                        if (showViral != null)
                        {
                            uri += "/" + showViral.ToString();
                            if (page != null)
                            {
                                uri += "/" + page;
                            }
                        }
                    }
                }
            }
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            return response["data"].ToObject<List<Image>>();
        }

        /// <summary>
        /// Gets the gallery
        /// </summary>
        /// <param name="section"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="showViral">Used only when section is "user"</param>
        /// <returns></returns>
        public static async Task<List<Image>> GetGallery(Section? section = null, Sort? sort = null, int? page = null, bool? showViral = null)
        {
            string uri = "gallery";
            if (section != null)
            {
                uri += "/" + section.ToString().ToLower();
                if (sort != null)
                {
                    if (page != null)
                    {
                        uri += "/" + page;
                    }
                }                
            }
            if (showViral != null)
                uri += ("?showViral=" + showViral.ToString().ToLower());
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            return response["data"].ToObject<List<Image>>();
        }



        public static async Task<List<Image>> GetSubreddditGallery(string subreddit, Sort? sort = null, Window? window = null, int? page = null)
        {
            //{ subreddit}/{ sort}/{ window}/{ page}
            string uri = "gallery/r/" + subreddit;
            if (sort != null)
            {
                uri += "/" + sort.ToString().ToLower();
                if (window != null)
                {
                    uri += "/" + window.ToString().ToLower();

                    if (page != null)
                    {
                        uri += "/" + page;
                    }
                }
            }
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            return response["data"].ToObject<List<Image>>();
        }

        public static async Task<List<Image>> GetSubreddditGallery(string subreddit, Sort? sort = null, int? page = null)
        {
            //{ subreddit}/{ sort}/{ window}/{ page}
            string uri = "gallery/r/" + subreddit;
            if (sort != null)
            {
                uri += "/" + sort.ToString().ToLower();
                if (page != null)
                {
                    uri += "/" + page;
                }
            }
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            return response["data"].ToObject<List<Image>>();
        }

        public static async Task<List<Comment>> GetComments(string imageId, Sort sort = Sort.Best)
        {
            //gallery/{id}/comments/{sort}
            string uri = "gallery/" + imageId + "/comments/" + sort.ToString().ToLower();
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            if (response["data"] == null || !(bool)response["success"])
                return null;
            return response["data"].ToObject<List<Comment>>();
        }

        public static async Task<List<Image>> SearchGallery(string query, Sort? sort = null, int? page = null )
        {
            //https://api.imgur.com/3/gallery/search/{sort}/{page}
            string uri = "gallery/search";
            if (sort != null)
            {
                uri += "/" + sort.ToString().ToLower();
                if (page != null)
                {
                    uri += "/" + page;
                }
            }
            uri = $"{uri}?q={query}";
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            return response["data"].ToObject<List<Image>>();
        }
            
        public static async Task Vote(string id, string vote)
        {
            string urlString = $"gallery/{id}/vote/{vote}";
            var result = await NetworkHelper.ExecutePostRequest(urlString, new JObject());
            return;
        }

        private static async Task<bool> Favourite(string id, string type)
        {
            string urlString = $"{type}/{id}/favorite";
            var result = await NetworkHelper.ExecutePostRequest(urlString, new JObject());
            if (result.HasValues && (bool)result["success"])
                return (string)result["data"] == "favorited";
            return false;
        }

        public static async Task<bool> FavouriteImage(string id)
        {
            return await Favourite(id, "image");
        }

        public static async Task<bool> FavouriteAlbum(string id)
        {
            return await Favourite(id, "album");
        }
    }
}
