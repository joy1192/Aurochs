using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public enum NotificationType
    {
        Unknown,
        Mention,
        Reblog,
        Favourite,
        Follow,
    }

    public class Notification
    {
        public int Id { get; set; }

        public NotificationType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public Account Account { get; set; }

        public Status Status { get; set; }
    }
}
