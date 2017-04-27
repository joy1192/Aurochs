using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Views.Utility
{
    public class EmojiUriResolver
    {
        public static EmojiUriResolver Resolver { get; set; } = new EmojiUriResolver();

        // TODO: 適切なインスタンス別のURIを返す（未実装）
        public virtual Uri CodeToUrl(string shortcode)
        {
            if (shortcode == ":white_check_mark:")
            {
                return new Uri("https://friends.nico/emoji/2705.svg");
            }
            else if (shortcode == ":nicoru:")
            {
                return new Uri("https://friends.nico/assets/nicoru-9bb19b5f004d40b5587f9b865c14c177a577369bd90fe4f41fbe55be36b71f28.svg");
            }
            return new Uri("https://friends.nico/assets/nicoru-9bb19b5f004d40b5587f9b865c14c177a577369bd90fe4f41fbe55be36b71f28.svg");
        }
    }
}
