using Aurochs.Desktop.Models.Contents;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ViewModels.Contents
{
    public class StatusViewModel : BindableBase
    {
        public string DisplayId
        {
            get { return _DisplayId; }
            set { SetProperty(ref _DisplayId, value); }
        }
        private string _DisplayId;

        public string Text
        {
            get { return _Text; }
            set { SetProperty(ref _Text, value); }
        }
        private string _Text;

        public string AvatarImageURI
        {
            get { return _AvatarImageURI; }
            set { SetProperty(ref _AvatarImageURI, value); }
        }
        private string _AvatarImageURI;

        public string CreateTime
        {
            get { return _CreateTime; }
            set { SetProperty(ref _CreateTime, value); }
        }
        private string _CreateTime;

        public string SourceAvatarImageURI
        {
            get { return _SourceAvatarImageURI; }
            set { SetProperty(ref _SourceAvatarImageURI, value); }
        }

        public string CreateDate { get; private set; }

        private string _SourceAvatarImageURI;


        public void Update(Status status)
        {
            if (status.Boost == null)
            {
                this.DisplayId = status.AccountScreenName;
                this.Text = status.Text;
                this.AvatarImageURI = status.AvatarImageURI;
                this.SourceAvatarImageURI = null;

                var localTime = status.CreateAt.ToLocalTime();

                this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
                this.CreateDate = (DateTime.Today == localTime.Date) ? string.Empty : $"{ localTime.Year}/{ localTime.Month}/{ localTime.Day}";
            }
            else
            {
                var boost = status.Boost;
                this.DisplayId = boost.AccountScreenName;
                this.Text = boost.Text;
                this.AvatarImageURI = boost.AvatarImageURI;
                this.SourceAvatarImageURI = status.AvatarImageURI;

                var localTime = status.CreateAt.ToLocalTime();

                this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
                this.CreateDate = (DateTime.Today == localTime.Date) ? string.Empty : $"{ localTime.Year}/{ localTime.Month}/{ localTime.Day}";
            }
        }
    }
}
