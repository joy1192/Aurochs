using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public enum AttachmentType
    {
        Unknown,
        Image,
        Video,
        GifVideo,
    }

    public class Attachment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public AttachmentType Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("remote_url")]
        public string RemoteUrl { get; set; }

        [JsonProperty("previre_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("text_url")]
        public string TextUrl { get; set; }
    }
}
