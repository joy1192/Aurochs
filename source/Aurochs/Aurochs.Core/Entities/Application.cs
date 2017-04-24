using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public class Application
    {
        /// <summary>
        /// アプリケーション名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// ApplicationのHomepageURL
        /// </summary>
        [JsonProperty("website")]
        public string Website { get; set; }
    }
}
