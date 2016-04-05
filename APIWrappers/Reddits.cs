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
    public class Reddits
    {
        public static async Task<List<Subreddit>> SearchSubreddits(string query)
        {
            string url = $"https://www.reddit.com/subreddits/search.json?q={query}";
            JObject result = await NetworkHelper.ExecuteRedditRequest(url);
            return result["data"]["children"].ToObject<List<Subreddit>>();
        }
    }
}
