using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public class Status
    {
        /// <summary>
        /// Status固有ID
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("in_reply_to_id")]
        public long? InReplyToId { get; set; }

        [JsonProperty("reblog")]
        public Status Reblog { get; set; }

        [JsonProperty("in_reply_to_account_id")]
        public long? InReplyToAccountId { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("reblog_count")]
        public long ReblogCount { get; set; }

        [JsonProperty("favorites_count")]
        public long FavouritesCount { get; set; }

        [JsonProperty("reblogged")]
        public bool? Reblogged { get; set; }

        [JsonProperty("favourited")]
        public bool? Favourited { get; set; }

        [JsonProperty("sensitive")]
        public bool? Sensitive { get; set; }

        [JsonProperty("spoiler_text")]
        public string SpoilerText { get; set; }

        [JsonProperty("visiblity")]
        public string Visibility { get; set; }

        [JsonProperty("media_attachments")]
        public IEnumerable<Attachment> MediaAttachments { get; set; }

        [JsonProperty("mentions")]
        public IEnumerable<Mention> Mentions { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<Tag> Tags { get; set; }

        [JsonProperty("application")]
        public Application Application { get; set; }
    }
}
