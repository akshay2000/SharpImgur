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
            var result = await NetworkHelper.ExecutePostRequest(url, new Newtonsoft.Json.Linq.JObject());
            return;
        }
    }
}
