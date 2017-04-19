using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Models.Contents
{
    public class Status
    {
        public Account Account { get; internal set; }

        public long? AccountId { get { return this.Account?.AccountId; } }

        public string UserScreenName { get { return this.Account?.ScreenAccountId; } }

        public string AvatarImageURI { get { return this.Account?.AvatarImageURI; } }

        public DateTime CreateAt { get; internal set; }

        public string Text { get; internal set; }

        public int StatusId { get; internal set; }

        public Status Boost { get; internal set; }

        public string AccountScreenName { get; internal set; }
    }
}
