using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Views.Utility
{
    public enum InlineContentType
    {
        Text,
        Link,
        Mension,
        HashTag,
    }

    public static class InlineGenerator
    {
        public static IEnumerable<(InlineContentType Type,T Inline)> Resolve<T>(string status, Func<InlineContentType, string, string, (InlineContentType Type, T Inline)> factory)
        {
            return null;
        }
    }
}
