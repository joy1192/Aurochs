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
using System.Windows;
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
        /// Statusを投稿したAccountのAccountNameを取得、または設定します
        /// </summary>
        public string AccountName
        {
            get { return _AccountName; }
            set { SetProperty(ref _AccountName, value); }
        }
        private string _AccountName;

        /// <summary>
        /// Statusを投稿したAccountのUserNameを取得、または設定します
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set { SetProperty(ref _UserName, value); }
        }
        private string _UserName;

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

        /// <summary>
        /// Statusを投稿したAccountの所属Instanceを取得、または設定します
        /// </summary>
        public string InstanceName
        {
            get { return _InstanceName; }
            set { SetProperty(ref _InstanceName, value); }
        }
        private string _InstanceName;

        /// <summary>
        /// 可視状態を取得、または設定します
        /// </summary>
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { SetProperty(ref _Visibility, value); }
        }
        private Visibility _Visibility;

        public IEnumerable<Uri> MediaAttachmentUris
        {
            get { return _MediaAttachmentUris; }
            set { SetProperty(ref _MediaAttachmentUris, value); }
        }
        private IEnumerable<Uri> _MediaAttachmentUris;

        public List<AttachmentViewModel> AttachmentUrls
        {
            get { return _AttachmentUrls; }
            set { SetProperty(ref _AttachmentUrls, value); }
        }
        private List<AttachmentViewModel> _AttachmentUrls = new List<AttachmentViewModel>();

        public StatusViewModel()
        {
        }

        public StatusViewModel(Status status)
        {
            this.Update(status);
        }

        private string AccountToInstanceName(Account account)
        {
            var accountName = account.AccountName;
            var userName = account.UserName;
            return accountName.Replace(userName, string.Empty);
        }

        public void Update(Status status)
        {
            StatusId = status.Id;

            var target = status.Reblog ?? status;

            this.AccountName = target.Account.AccountName;
            this.UserName = target.Account.UserName;
            this.InstanceName = AccountToInstanceName(target.Account);
            this.DisplayName = target.Account.DisplayName;
            this.Text = target.Content;
            this.SpoilerText = target.SpoilerText;
            this.IsContentsWarning = !string.IsNullOrEmpty(target.SpoilerText);
            this.AvatarImageURI = ToFullUrl(target.Account.AvatarImageUrl);
            this.AttachmentUrls = target.MediaAttachments.Select(x => new AttachmentViewModel(x.Url)).ToList();
            this.MediaAttachmentUris = target.MediaAttachments.SelectMany(x => AttachmentToUrls(x)).ToList();
            var localTime = target.CreatedAt.ToLocalTime();

            this.CreateTime = $"{localTime.Hour:00}:{localTime.Minute:00}";
            this.CreateDate = (DateTime.Today == localTime.Date) ? string.Empty : $"{ localTime.Year}/{ localTime.Month}/{ localTime.Day}";

            this.SourceAvatarImageURI = status.Reblog != null ? ToFullUrl(status.Account.AvatarImageUrl) : null;
            this.IsReblog = status.Reblog != null;
        }

        private static IEnumerable<Uri> AttachmentToUrls(Attachment attachment)
        {
            if (Uri.TryCreate(attachment.RemoteUrl, UriKind.Absolute, out Uri remoteUrl))
            {
                yield return remoteUrl;
            }

            if (Uri.TryCreate(attachment.TextUrl, UriKind.Absolute, out Uri textUrl))
            {
                yield return textUrl;
            }

            if (Uri.TryCreate(attachment.Url, UriKind.Absolute, out Uri rawUrl))
            {
                yield return rawUrl;
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
