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
        public int Id { get; set; }

        /// <summary>
        /// アカウントのプロフィールURLを取得、または設定します
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// アカウントのユーザー名を取得、または設定します
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// アカウント名を取得、または設定します
        /// </summary>
        public string AccountName { get; set; }
    }
}
