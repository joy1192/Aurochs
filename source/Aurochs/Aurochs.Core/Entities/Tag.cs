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
        public string Name { get; set; }

        /// <summary>
        /// ハッシュタグのURLを取得、または設定します
        /// </summary>
        public string Url { get; set; }
    }
}
