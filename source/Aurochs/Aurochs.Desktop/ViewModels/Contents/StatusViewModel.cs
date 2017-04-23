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
        /// <summary>
        /// Statsuを一意に識別する識別子を取得します
        /// TODO: インスタンス情報が欠損しているため、現在厳密にはこのプロパティ単体では一意には識別できない
        /// </summary>
        public long StatusId { get; private set; }

        /// <summary>
        /// Statusを投稿したAccountのID名を取得、または設定します
        /// </summary>
        public string AccountName
        {
            get { return _AccountName; }
            set { SetProperty(ref _AccountName, value); }
        }
        private string _AccountName;

        /// <summary>
        /// Statusを投稿したAccountの表示名を取得、または設定します
        /// </summary>
        public string DisplayName
        {
            get { return _DisplayName; }
            set { SetProperty(ref _DisplayName, value); }
        }
        private string _DisplayName;

        /// <summary>
        /// Statusのテキスト内容を取得、または設定します
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { SetProperty(ref _Text, value); }
        }
        private string _Text;

        /// <summary>
        /// ContentsWarningの注意文を取得、または設定します
        /// </summary>
        public string SpoilerText
        {
            get { return _SpoilerText; }
            set { SetProperty(ref _SpoilerText, value); }
        }
        private string _SpoilerText;

        /// <summary>
        /// Statusを投稿したAccountのAvatorUrlを取得、または設定します
        /// </summary>
        public string AvatarImageURI
        {
            get { return _AvatarImageURI; }
            set { SetProperty(ref _AvatarImageURI, value); }
        }
        private string _AvatarImageURI;

        /// <summary>
        /// Statsuが作成された時間を取得、または設定します
        /// </summary>
        public string CreateTime
        {
            get { return _CreateTime; }
            set { SetProperty(ref _CreateTime, value); }
        }
        private string _CreateTime;

        public string CreateDate { get; private set; }

        /// <summary>
        /// Reblog元のAccountのAvatorUrlを取得、または設定します
        /// </summary>
        public string SourceAvatarImageURI
        {
            get
            {
                return _SourceAvatarImageURI;
            }
            set { SetProperty(ref _SourceAvatarImageURI, value); }
        }
        private string _SourceAvatarImageURI;

        /// <summary>
        /// ReblogされたStatusであるかを取得、または設定します
        /// </summary>
        public bool IsReblog
        {
            get { return _IsReblog; }
            set { SetProperty(ref _IsReblog, value); }
        }
        private bool _IsReblog;

        /// <summary>
        /// CWであるかを取得、または設定します
        /// </summary>
        public bool IsContentsWarning
        {
            get { return _IsContentsWarning; }
            set { SetProperty(ref _IsContentsWarning, value); }
        }
        private bool _IsContentsWarning;

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
                this.AccountName = status.Account.AccountName;
                this.DisplayName = status.Account.DisplayName;
                this.Text = status.Content;
                this.SpoilerText = status.SpoilerText;
                this.IsContentsWarning = !string.IsNullOrEmpty(status.SpoilerText);
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
                this.AccountName = reblog.Account.AccountName;
                this.DisplayName = reblog.Account.UserName;
                this.Text = reblog.Content;
                this.SpoilerText = status.SpoilerText;
                this.IsContentsWarning = !string.IsNullOrEmpty(status.SpoilerText);
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
                return $"https://friends.nico{url}"; // TODO: 特定インスタンスドメイン直書き直す

            return url;
        }
    }
}
