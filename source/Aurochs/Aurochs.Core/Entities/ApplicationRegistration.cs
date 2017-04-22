using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public class ApplicationRegistration
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("instance")]
        public string Instance { get; set; }

        [JsonIgnore]
        public bool AllowRead { get; set; }

        [JsonIgnore]
        public bool AllowWrite { get; set; }

        [JsonIgnore]
        public bool AllowFollow { get; set; }
    }
}
