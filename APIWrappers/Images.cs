using Newtonsoft.Json.Linq;
using SharpImgur.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SharpImgur.APIWrappers
{
    public static class Images
    {
        public static async Task UploadImage(StorageFile file, string albumId = null, string type = null, string title = null, string description = null)
        {
            string base64image = Convert.ToBase64String(await ReadFile(file));
            JObject payload = new JObject();
            payload["image"] = base64image;
            if (albumId != null) payload["album"] = albumId;
            if (type != null) payload["type"] = type;
            if (title != null) payload["title"] = title;
            if (description != null) payload["description"] = description;
            string uri = "upload";
            JObject result = await NetworkHelper.ExecutePostRequest(uri, payload);
        }

        private static async Task<byte[]> ReadFile(StorageFile file)
        {
            byte[] fileBytes = null;
            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            return fileBytes;
        }

        public static async Task UpdateImage(string id, string title, string description)
        {
            string uri = $"image/{id}";
            JObject payload = new JObject();
            payload["title"] = title;
            payload["description"] = description;
            JObject result = await NetworkHelper.ExecutePostRequest(uri, payload);
        }
    }
}
