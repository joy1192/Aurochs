using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

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
        private static bool IsHCard(HtmlNode node)
        {
            if (!node.HasAttributes)
                return false;

            if (node.Name != "span")
                return false;

            var classvalue = node.Attributes["class"];
            if (classvalue == null)
                return false;

            return (classvalue.Value == "h-card");
        }

        private static bool IsMention(HtmlNode node)
        {
            if (!node.HasAttributes)
                return false;

            if (node.Name != "a")
                return false;

            var hrefvalue = node.Attributes["href"];
            if (hrefvalue == null)
                return false;


            var classvalue = node.Attributes["class"];
            if (classvalue == null)
                return false;

            return (classvalue.Value == "u-url mention");
        }

        private static bool IsText(HtmlNode node)
        {
            return node.Name == "#text";
        }

        private static bool IsNewLine(HtmlNode node)
        {
            return node.Name == "br";
        }

        private static bool IsHyperlink(HtmlNode node)
        {
            if (!node.HasAttributes)
                return false;

            if (node.Name != "a")
                return false;

            var hrefvalue = node.Attributes["href"];
            if (hrefvalue == null)
                return false;

            return true;
        }

        public static IEnumerable<(InlineContentType Type, T Inline)> Resolve<T>(string status, Func<InlineContentType, string, string, (InlineContentType Type, T Inline)> factory)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(status);
            foreach (var element in doc.DocumentNode.ChildNodes.SelectMany(x => x.ChildNodes))
            {
                if (IsHCard(element))
                {
                    var link = element.Descendants("a").Single();
                    if (IsMention(link))
                    {
                        var text = link.InnerText;
                        var url = link.Attributes["href"].Value;

                        yield return factory(InlineContentType.Mension, text, url);
                    }
                }
                else if (IsText(element))
                {
                    yield return factory(InlineContentType.Text, element.InnerText, null);
                }
                else if (IsNewLine(element))
                {
                    yield return factory(InlineContentType.Text, "\n", null);
                }
                else if (IsHyperlink(element))
                {
                    var text = element.Descendants("span").
                        Where(x => x.Attributes["class"]?.Value != "invisible").
                        Select(x => x.InnerText).
                        Single();
                    var url = element.Attributes["href"].Value;

                    yield return factory(InlineContentType.Link, text.ToString(), url);
                }
            }
        }
    }
}
