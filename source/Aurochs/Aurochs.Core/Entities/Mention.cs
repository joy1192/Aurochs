using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public class Mention
    {
        /// <summary>
        /// アカウントのIdを取得、または設定します
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// アカウントのプロフィールURLを取得、または設定します
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// アカウントのユーザー名を取得、または設定します
        /// </summary>
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// アカウント名を取得、または設定します
        /// </summary>
        [JsonProperty("account_name")]
        public string AccountName { get; set; }
    }
}
