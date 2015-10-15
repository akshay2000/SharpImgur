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
    public static class Topic
    {
        public static async Task<List<Image>> GetTopicGallery(int topicId, Sort? sort = null, Window? window = null, int? page = null)
        {
            //{topicId}/{sort}/{window}/{page}
            string uri = "topics/" + topicId;
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
    }
}
