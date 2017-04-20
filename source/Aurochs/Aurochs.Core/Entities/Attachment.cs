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
        public int Id { get; set; }

        public AttachmentType Type { get; set; }

        public string Url { get; set; }

        public string RemoteUrl { get; set; }

        public string PreviewUrl { get; set; }

        public string TextUrl { get; set; }
    }
}
