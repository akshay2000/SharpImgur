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
    public static class Comments
    {
        public static async Task<Response<bool>> Vote(long commentId, string vote)
        {
            string url = $"comment/{commentId}/vote/{vote}";
            return await NetworkHelper.PostRequest<bool>(url, new JObject());
        }

        public static async Task<Response<long?>> CreateComment(string comment, string imageId, long? parentId = null)
        {
            JObject payload = new JObject();
            payload["image_id"] = imageId;
            payload["comment"] = comment;
            if (parentId != null)
                payload["parent_id"] = parentId;
            string url = "comment";
            var response = await NetworkHelper.PostRequest<dynamic>(url, payload);
            Response<long?> ret = new Response<long?> { Error = response.Error, IsError = response.IsError, Message = response.Message };
            if (!response.IsError)
                ret.Content = (long?)((JObject)response.Content)["id"];
            return ret;
        }
    }
}
