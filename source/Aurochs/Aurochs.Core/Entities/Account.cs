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
        public long Id { get; set; }

        /// <summary>
        /// ユーザー名を取得、または設定します
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// アカウント名を取得、または設定します
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// アカウントの表示名を取得、または設定します
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Bioを取得、または設定します
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// プロフィールURLを取得、または設定します
        /// </summary>
        public string ProfileUrl { get; set; }

        /// <summary>
        /// アイコン画像URLを取得、または設定します
        /// </summary>
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// ヘッダー画像URLを取得、または設定します
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 非公開アカウントであるかを取得、または設定します
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// アカウントの作成日時を取得、または設定します
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// アカウントがフォローされている数を取得、または設定します
        /// </summary>
        public long FollowersCount { get; set; }

        /// <summary>
        /// フォロー中のアカウントの数を取得、または設定します
        /// </summary>
        public long FollowingCount { get; set; }

        /// <summary>
        /// StatusのPost数を取得、または設定します
        /// </summary>
        public long StatusesCount { get; set; }
    }
}
