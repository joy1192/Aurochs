using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public class Account
    {
        /// <summary>
        /// アカウント固有IDを取得、または設定します
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// ユーザー名を取得、または設定します
        /// </summary>
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// アカウント名を取得、または設定します
        /// </summary>
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        /// <summary>
        /// アカウントの表示名を取得、または設定します
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Bioを取得、または設定します
        /// </summary>
        [JsonProperty("note")]
        public string Note { get; set; }

        /// <summary>
        /// プロフィールURLを取得、または設定します
        /// </summary>
        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        /// <summary>
        /// アイコン画像URLを取得、または設定します
        /// </summary>
        [JsonProperty("avatar_url")]
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// ヘッダー画像URLを取得、または設定します
        /// </summary>
        [JsonProperty("header_url")]
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 非公開アカウントであるかを取得、または設定します
        /// </summary>
        [JsonProperty("locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// アカウントの作成日時を取得、または設定します
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// アカウントがフォローされている数を取得、または設定します
        /// </summary>
        [JsonProperty("followers_count")]
        public long FollowersCount { get; set; }

        /// <summary>
        /// フォロー中のアカウントの数を取得、または設定します
        /// </summary>
        [JsonProperty("following_count")]
        public long FollowingCount { get; set; }

        /// <summary>
        /// StatusのPost数を取得、または設定します
        /// </summary>
        [JsonProperty("statuses_count")]
        public long StatusesCount { get; set; }
    }
}
