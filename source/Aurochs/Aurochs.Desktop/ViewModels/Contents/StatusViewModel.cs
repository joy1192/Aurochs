using Aurochs.Core.Entities;
using Aurochs.Desktop.Views.Utility;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Aurochs.Desktop.ViewModels.Contents
{
    public class StatusViewModel : BindableBase
    {
        public long StatusId { get; set; }

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
            get
            {
                return _SourceAvatarImageURI;
            }
            set { SetProperty(ref _SourceAvatarImageURI, value); }
        }

        public string CreateDate { get; private set; }

        private string _SourceAvatarImageURI;

        public bool IsReblog
        {
            get { return _IsReblog; }
            set { SetProperty(ref _IsReblog, value); }
        }
        private bool _IsReblog;

        public StatusViewModel()
        {

        }

        public StatusViewModel(Status status)
        {
            this.Update(status);
        }

        public void Update(Status status)
        {
            StatusId = status.Id;

            if (status.Reblog == null)
            {
                this.DisplayId = status.Account.AccountName;
                this.Text = status.Content;
                this.AvatarImageURI = ToFullUrl(status.Account.AvatarImageUrl);
                this.SourceAvatarImageURI = null;
                this.IsReblog = false;

                var localTime = status.CreatedAt.ToLocalTime();

                this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
                this.CreateDate = (DateTime.Today == localTime.Date) ? string.Empty : $"{ localTime.Year}/{ localTime.Month}/{ localTime.Day}";
            }
            else
            {
                var reblog = status.Reblog;
                this.DisplayId = reblog.Account.AccountName;
                this.Text = reblog.Content;
                this.AvatarImageURI = ToFullUrl(reblog.Account.AvatarImageUrl);
                this.SourceAvatarImageURI = ToFullUrl(status.Account.AvatarImageUrl);
                this.IsReblog = true;

                var localTime = status.CreatedAt.ToLocalTime();

                this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
                this.CreateDate = (DateTime.Today == localTime.Date) ? string.Empty : $"{ localTime.Year}/{ localTime.Month}/{ localTime.Day}";
            }
        }


        private string ToFullUrl(string url)
        {
            if (url is null)
                return null;

            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                return url;

            if (url == "/avatars/original/missing.png")
                return $"https://{InstanceMetadata.InstanceName}{url}";

            return url;
        }
    }
}
