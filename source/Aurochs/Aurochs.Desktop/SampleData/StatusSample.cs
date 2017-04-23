using Aurochs.Desktop.ViewModels.Contents;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.SampleData
{
    public class StatusSample : StatusViewModel
    {
        public StatusSample()
        {
            this.AccountName = "joy1192";
            this.Text = "開発なう";
            var localTime = DateTime.Now;
            this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
            this.AvatarImageURI = "https://pbs.twimg.com/profile_images/2932172472/c4a62488f163651a929158d7542ae805_normal.png";
        }
    }
}
