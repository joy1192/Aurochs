﻿using Aurochs.Core.Entities;
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
    public static class MastnetToLocalConverter
    {
        public static Status ConvertStatus(this Mastonet.Entities.Status s)
        {
            return new Status
            {
                Id = s.Id,
                Uri = s.Uri,
                Url = s.Url,
                Account = s.Account?.ConvertAccount(),
                InReplyToId = s.InReplyToId,
                InReplyToAccountId = s.InReplyToAccountId,
                Reblog = s.Reblog?.ConvertStatus(),
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                ReblogCount = s.ReblogCount,
                FavouritesCount = s.FavouritesCount,
                Reblogged = s.Reblogged,
                Favourited = s.Favourited,
                Sensitive = s.Sensitive,
                SpoilerText = s.SpoilerText,
                Visibility = s.Visibility,
                MediaAttachments = s.MediaAttachments.Select(x => x.ConvertAttachment()).ToList(),
                Mentions = s.Mentions.Select(x => x.ConvertMention()).ToList(),
                Tags = s.Tags.Select(x => x.ConvertTag()).ToList(),
                Application = s.Application?.ConvertApplication()
            };
        }

        public static Account ConvertAccount(this Mastonet.Entities.Account account)
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

        public static Attachment ConvertAttachment(this Mastonet.Entities.Attachment attachment)
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

        public static Mention ConvertMention(this Mastonet.Entities.Mention m)
        {
            return new Mention
            {
                Id = m.Id,
                Url = m.Url,
                UserName = m.UserName,
                AccountName = m.AccountName
            };
        }

        public static Tag ConvertTag(this Mastonet.Entities.Tag tag)
        {
            return new Tag
            {
                Name = tag.Name,
                Url = tag.Url
            };
        }

        public static Application ConvertApplication(this Mastonet.Entities.Application application)
        {
            return new Application
            {
                Name = application.Name,
                Website = application.Website,
            };
        }
    }
}