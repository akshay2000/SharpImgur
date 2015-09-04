using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpImgur.Models;
using Newtonsoft.Json.Linq;
using SharpImgur.Helpers;

namespace SharpImgur.APIWrappers
{
    public class Album
    {
        public static async Task<List<Image>> GetImages(string albumId)
        {
            string uri = "album/" + albumId + "/images";
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            return response["data"].ToObject<List<Image>>();
        }
    }
}
