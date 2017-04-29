using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Views.Utility
{
    public class EmojiUriResolver
    {
        public static EmojiUriResolver Resolver { get; set; } = new EmojiUriResolver();

        public class EmojiData
        {
            [JsonProperty("emoji")]
            public string Emoji { get; set; }

            [JsonProperty("shortcode")]
            public string ShortCode {
                get
                {
                    return $":{_shortCode}:";
                }
                set
                {
                    _shortCode = value;
                }
            }
            private string _shortCode;

            [JsonProperty("codepoint")]
            public string CodePoint { get; set; }
        }

        private IEnumerable<EmojiData> Emojis { get; }

        public EmojiUriResolver()
        {
            using (var reader = new StreamReader("source.json"))
            using (var jReader = new JsonTextReader(reader))
            {
                var serializer = JsonSerializer.Create();
                Emojis = serializer.Deserialize<IEnumerable<EmojiData>>(jReader).OrderBy(x => int.Parse(x.CodePoint,NumberStyles.HexNumber)).ToList();
            }
        }

        // TODO: 適切なインスタンス別のURIを返す（未実装）
        public virtual Uri CodeToUrl(string shortcode)
        {
            var target = Emojis.FirstOrDefault(x => x.ShortCode == shortcode);
            if (target == null)
            {
                if (Regex.IsMatch(shortcode, ":nicoru([0-9]*):"))
                {
                    return new Uri("https://friends.nico/assets/nicoru-9bb19b5f004d40b5587f9b865c14c177a577369bd90fe4f41fbe55be36b71f28.svg");
                }
                else
                {
                    return null;
                }
            }
            return new Uri($"https://friends.nico/emoji/{target.CodePoint}.svg");
        }
    }
}
