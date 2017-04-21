using Aurochs.Core.Entities;
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
            if (status.Reblog == null)
            {
                this.DisplayId = status.Account.AccountName;
                this.Text = status.Content;
                this.AvatarImageURI = status.Account.AvatarImageUrl;
                this.SourceAvatarImageURI = null;

                var localTime = status.CreatedAt.ToLocalTime();

                this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
                this.CreateDate = (DateTime.Today == localTime.Date) ? string.Empty : $"{ localTime.Year}/{ localTime.Month}/{ localTime.Day}";
            }
            else
            {
                var reblog = status.Reblog;
                this.DisplayId = reblog.Account.AccountName;
                this.Text = reblog.Content;
                this.AvatarImageURI = reblog.Account.AvatarImageUrl;
                this.SourceAvatarImageURI = status.Account.AvatarImageUrl;

                var localTime = status.CreatedAt.ToLocalTime();

                this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
                this.CreateDate = (DateTime.Today == localTime.Date) ? string.Empty : $"{ localTime.Year}/{ localTime.Month}/{ localTime.Day}";
            }
        }

        //public string ParseContent(string rawContent)
        //{
        //    var reg = new Regex("<a href=\"(?<url>.*?)\".* class=\"u-url mention\".*?>@\\<span\\>(?<text>.*?)\\<\\/span\\>\\<\\/a\\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //    foreach (var match in reg.Matches(rawContent).OfType<Match>())
        //    {
        //        var mention_url = match.Groups["url"].Value;
        //        var fullId = Regex.Replace(mention_url, "^http(s)?://(?<Domain>.*?)/@(?<Account>.*)$", "${Account}@${Domain}");
        //        rawContent = rawContent.Replace(match.Value, fullId);
        //    }

        //    var text = rawContent.
        //        Replace("<br>", "\n").
        //        Replace("<br/>", "\n").
        //        Replace("<br />", "\n").
        //        Replace("</p><p>", "\n\n");

        //    // Tag除去
        //    var formattedText =  Regex.Replace(text, "<.*?>", String.Empty);

        //    return HttpUtility.HtmlDecode(formattedText);
        //}
    }
}
