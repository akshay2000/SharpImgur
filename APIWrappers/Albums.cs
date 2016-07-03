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
        public static async Task<Album> GetAlbum(string albumId)
        {
            string uri = "album/" + albumId;
            JObject response = await NetworkHelper.ExecuteRequest(uri);
            if (response.HasValues)
                return response["data"].ToObject<Album>();
            else
                return new Album();
        }

        public static async Task<List<Image>> GetImages(string albumId)
        {
            string uriString = $"album/{albumId}/images";
            JObject response = await NetworkHelper.ExecuteRequest(uriString);
            return response["data"].ToObject<List<Image>>();
        }

        public static async Task<Response<bool>> DeleteAlbum(string id)
        {
            string uri = $"album/{id}";
            return await NetworkHelper.DeleteRequest<bool>(uri);
        }
    }
}
