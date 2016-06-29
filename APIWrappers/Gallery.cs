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
        public static async Task<Response<List<Image>>> GetGallery(Section? section = null, Sort? sort = null, Window? window = null, bool? showViral = null, int? page = null)
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
            return await NetworkHelper.GetRequest<List<Image>>(uri);
        }

        /// <summary>
        /// Gets the gallery
        /// </summary>
        /// <param name="section"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="showViral">Used only when section is "user"</param>
        /// <returns></returns>
        public static async Task<Response<List<Image>>> GetGallery(Section? section = null, Sort? sort = null, int? page = null, bool? showViral = null)
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
            var response = await NetworkHelper.GetRequest<List<Image>>(uri);
            return response;
        }

        public static async Task<Response<List<Image>>> GetSubreddditGallery(string subreddit, Sort? sort = null, Window? window = null, int? page = null)
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
            return await NetworkHelper.GetRequest<List<Image>>(uri);
        }

        public static async Task<Response<List<Image>>> GetSubreddditGallery(string subreddit, Sort? sort = null, int? page = null)
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
            return await NetworkHelper.GetRequest<List<Image>>(uri);
        }

        public static async Task<Response<List<Comment>>> GetComments(string imageId, Sort sort = Sort.Best)
        {
            //gallery/{id}/comments/{sort}
            string uri = "gallery/" + imageId + "/comments/" + sort.ToString().ToLower();
            return await NetworkHelper.GetRequest<List<Comment>>(uri);
        }

        public static async Task<Response<List<Image>>> SearchGallery(string query, Sort? sort = null, int? page = null )
        {
            //gallery/search/{sort}/{page}
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
            return await NetworkHelper.GetRequest<List<Image>>(uri);
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
