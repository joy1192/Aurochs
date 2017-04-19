using Aurochs.Desktop.Models.Contents;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.SampleData
{
    public class TimelineSample : BindableBase
    {
        public ObservableCollection<StatusSample> Toots
        {
            get { return _Tweets; }
            set { SetProperty(ref _Tweets, value); }
        }
        private ObservableCollection<StatusSample> _Tweets = new ObservableCollection<StatusSample>();


        public TimelineSample()
        {
            Toots.Add(new StatusSample());
            Toots.Add(new StatusSample());
            Toots.Add(new StatusSample());

            var me = new Account()
            {
                AccountId = -1,
                ScreenAccountId = "joy1192",
                AvatarImageURI = "https://pbs.twimg.com/profile_images/2932172472/c4a62488f163651a929158d7542ae805_normal.png"
            };

            Toots[0].Update(new Status
            {
                Account = me,
                CreateAt = (DateTime.Now - TimeSpan.FromDays(1)),
                Text = "() -> [] %*+",
                StatusId = -1,
                Boost = null
            });

            Toots[1].Update(new Status
            {
                Account = me,
                CreateAt = DateTime.Now,
                Text = "パオーン",
                StatusId = -1,
                Boost = new Status
                {
                    Account = me,
                    CreateAt = (DateTime.Now - TimeSpan.FromDays(10)),
                    Text = "Boooosted",
                    StatusId = -1,
                    Boost = null
                }
            });

            Toots[2].Update(new Status
            {
                Account = me,
                CreateAt = DateTime.Now,
                Text = "mastodon",
                StatusId = -1,
                Boost = null
            });
        }
    }
}
