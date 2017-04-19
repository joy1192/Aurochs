using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Models.Contents
{
    public class Account
    {
        public int AccountId { get; internal set; }
        public string ScreenAccountId { get; internal set; }
        public string AvatarImageURI { get; internal set; }
    }
}
