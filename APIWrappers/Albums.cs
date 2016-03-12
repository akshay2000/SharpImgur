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
    public class Albums
    {
        public static async Task<Models.Album> GetAlbum(string albumId)
        {
            string uri = "album/" + albumId;
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            if (response.HasValues)
                return response["data"].ToObject<Models.Album>();
            else
                return new Models.Album();
        }
    }
}
