using Aurochs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aurochs.Linkage.Converter
{
    /// <summary>
    /// 外部ライブラリ(Mastonet)のデータ型をローカルのデータ型に変換する
    /// Converter
    /// </summary>
    public static class MastnetExtensions
    {
        public static Status ToStatus(this Mastonet.Entities.Status s)
        {
            return new Status
            {
                Id = s.Id,
                Uri = s.Uri,
                Url = s.Url,
                Account = s.Account?.ToAccount(),
                InReplyToId = s.InReplyToId,
                InReplyToAccountId = s.InReplyToAccountId,
                Reblog = s.Reblog?.ToStatus(),
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                ReblogCount = s.ReblogCount,
                FavouritesCount = s.FavouritesCount,
                Reblogged = s.Reblogged,
                Favourited = s.Favourited,
                Sensitive = s.Sensitive,
                SpoilerText = s.SpoilerText,
                Visibility = s.Visibility,
                MediaAttachments = s.MediaAttachments.Select(x => x.ToAttachment()).ToList(),
                Mentions = s.Mentions.Select(x => x.ToMention()).ToList(),
                Tags = s.Tags.Select(x => x.ToTag()).ToList(),
                Application = s.Application?.ToApplication()
            };
        }

        public static Account ToAccount(this Mastonet.Entities.Account account)
        {
            return new Account
            {
                Id = account.Id,
                UserName = account.UserName,
                AccountName = account.AccountName,
                DisplayName = account.DisplayName,
                Note = account.Note,
                ProfileUrl = account.ProfileUrl,
                AvatarImageUrl = account.AvatarUrl,
                HeaderImageUrl = account.HeaderUrl,
                Locked = account.Locked,
                CreatedAt = account.CreatedAt,
                FollowersCount = account.FollowersCount,
                FollowingCount = account.FollowingCount,
                StatusesCount = account.StatusesCount
            };
        }

        public static Attachment ToAttachment(this Mastonet.Entities.Attachment attachment)
        {
            AttachmentType StringToAttachmentType(string type)
            {
                switch (type)
                {
                    case "image":
                        return AttachmentType.Image;
                    case "video":
                        return AttachmentType.Video;
                    case "gifv":
                        return AttachmentType.GifVideo;
                    default:
                        return AttachmentType.Unknown;
                }
            }

            return new Attachment
            {
                Id = attachment.Id,
                Type = StringToAttachmentType(attachment.Type),
                Url = attachment.Url,
                RemoteUrl = attachment.RemoteUrl,
                PreviewUrl = attachment.PreviewUrl,
                TextUrl = attachment.TextUrl
            };
        }

        public static Mention ToMention(this Mastonet.Entities.Mention m)
        {
            return new Mention
            {
                Id = m.Id,
                Url = m.Url,
                UserName = m.UserName,
                AccountName = m.AccountName
            };
        }

        public static Tag ToTag(this Mastonet.Entities.Tag tag)
        {
            return new Tag
            {
                Name = tag.Name,
                Url = tag.Url
            };
        }

        public static Application ToApplication(this Mastonet.Entities.Application application)
        {
            return new Application
            {
                Name = application.Name,
                Website = application.Website,
            };
        }

        public static Notification ToNotification(this Mastonet.Entities.Notification notification)
        {
            NotificationType StringToNotificationType(string type)
            {
                switch (type)
                {
                    case "mention":
                        return NotificationType.Mention;
                    case "reblog":
                        return NotificationType.Reblog;
                    case "favourite":
                        return NotificationType.Favourite;
                    case "follow":
                        return NotificationType.Follow;
                    default:
                        return NotificationType.Unknown;
                }
            }

            return new Notification
            {
                Id = notification.Id,
                Type = StringToNotificationType(notification.Type),
                CreatedAt = notification.CreatedAt,
                Account = notification.Account.ToAccount(),
                Status = notification.Status.ToStatus()
            };
        }

        public static ApplicationRegistration ToAppRegistration(this Mastonet.Entities.AppRegistration registration)
        {
            return new ApplicationRegistration
            {
                Id = registration.Id,
                RedirectUri = registration.RedirectUri,
                ClientId = registration.ClientId,
                ClientSecret = registration.ClientSecret,
                Instance = registration.Instance,
                AllowRead = registration.Scope.HasFlag(Mastonet.Scope.Read),
                AllowWrite = registration.Scope.HasFlag(Mastonet.Scope.Write),
                AllowFollow = registration.Scope.HasFlag(Mastonet.Scope.Follow)
            };
        }

        public static Mastonet.Entities.AppRegistration ToMastonetAppRegistration(this ApplicationRegistration registration)
        {
            Mastonet.Scope GetScope(bool read, bool write, bool follow)
            {
                var scope = Mastonet.Scope.Read | Mastonet.Scope.Write | Mastonet.Scope.Follow;
                if (!read)
                {
                    scope = scope & ~Mastonet.Scope.Read;
                }
                if (!write)
                {
                    scope = scope & ~Mastonet.Scope.Write;
                }
                if (!follow)
                {
                    scope = scope & ~Mastonet.Scope.Follow;
                }
                return scope;
            }

            return new Mastonet.Entities.AppRegistration
            {
                Id = (int)registration.Id,
                RedirectUri = registration.RedirectUri,
                ClientId = registration.ClientId,
                ClientSecret = registration.ClientSecret,
                Instance = registration.Instance,
                Scope = GetScope(registration.AllowRead, registration.AllowWrite, registration.AllowFollow)
            };
        }
    }
}
