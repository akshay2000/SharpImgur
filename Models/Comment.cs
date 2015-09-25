using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImgur.Models
{
    public class Comment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }

        [JsonProperty("comment")]
        public string CommentText { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("author_id")]
        public int AuthorId { get; set; }

        [JsonProperty("on_album")]
        public bool OnAlbum { get; set; }

        [JsonProperty("album_cover")]
        public object AlbumCover { get; set; }

        [JsonProperty("ups")]
        public int Ups { get; set; }

        [JsonProperty("downs")]
        public int Downs { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("datetime")]
        public int Datetime { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("vote")]
        public object Vote { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("children")]
        public IList<Comment> Children { get; set; }
    }
}
