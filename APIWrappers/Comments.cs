using Newtonsoft.Json.Linq;
using SharpImgur.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImgur.APIWrappers
{
    public static class Comments
    {
        public static async Task Vote(long commentId, string vote)
        {
            string url = $"comment/{commentId}/vote/{vote}";
            var result = await NetworkHelper.ExecutePostRequest(url, new JObject());
            return;
        }

        public static async Task<long?> CreateComment(string comment, string imageId, string parentId = null)
        {
            JObject payload = new JObject();
            payload["image_id"] = imageId;
            payload["comment"] = comment;
            if (parentId != null)
                payload["parent_id"] = parentId;
            string url = "comment";
            var result = await NetworkHelper.ExecutePostRequest(url, payload);
            if (result.HasValues && (bool)result["success"])
                return (long)result["data"]["id"];
            else
                return null;
        }
    }
}
