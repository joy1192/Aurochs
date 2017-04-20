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
        public long Id { get; set; }

        public string Uri { get; set; }

        public string Url { get; set; }

        public Account Account { get; set; }

        public long? InReplyToId { get; set; }

        public Status Reblog { get; set; }

        public long? InReplyToAccountId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public long ReblogCount { get; set; }

        public long FavouritesCount { get; set; }

        public bool? Reblogged { get; set; }

        public bool? Favourited { get; set; }

        public bool? Sensitive { get; set; }

        public string SpoilerText { get; set; }

        public string Visibility { get; set; }

        public IEnumerable<Attachment> MediaAttachments { get; set; }

        public IEnumerable<Mention> Mentions { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
        
        public Application Application { get; set; }
    }
}
