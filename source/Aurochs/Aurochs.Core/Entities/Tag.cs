using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public class Tag
    {
        /// <summary>
        /// ハッシュタグ名を取得、または設定します
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// ハッシュタグのURLを取得、または設定します
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
