using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using System.Web;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Aurochs.Desktop.Views.Utility
{
    public enum InlineContentType
    {
        Text,
        Link,
        Mension,
        HashTag,
        Emoji,
    }

    public static class InlineResolver
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

        private static bool IsHashTag(HtmlNode node)
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

            return (classvalue.Value == "mention hashtag");
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

        public static IEnumerable<(InlineContentType Type, T Inline)> Resolve<T>(string status, IEnumerable<Uri> mediaUris, Func<InlineContentType, string, string, (InlineContentType Type, T Inline)> factory)
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
                    var text = HttpUtility.HtmlDecode(element.InnerText);
                    
                    foreach (var result in MakeInlineTypedDataset(text))
                    {
                        yield return factory(result.Type, result.Text, null);
                    }
                    
                }
                else if (IsNewLine(element))
                {
                    yield return factory(InlineContentType.Text, "\n", null);
                }
                else if (IsHashTag(element))
                {
                    string text = HttpUtility.HtmlDecode(element.InnerText);
                    string url = url = element.Attributes["href"].Value;

                    yield return factory(InlineContentType.HashTag, text, url);
                }
                else if (IsHyperlink(element))
                {
                    string text = null;
                    try
                    {
                        var innerText = element.Descendants("span").
                        Where(x => x.Attributes["class"]?.Value != "invisible").
                        Select(x => x.InnerText).
                        Select(x => HttpUtility.HtmlDecode(x)).ToArray();

                        text = innerText.SingleOrDefault();
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError($"{element}:{Environment.NewLine}{e}");
                    }

                    var url = element.Attributes["href"].Value;

                    if (Uri.TryCreate(url, UriKind.Absolute, out Uri wellFormedUri))
                    {
                        var hadMedia = mediaUris.Contains(wellFormedUri);

                        // MediaUrisに含まれていればUriとして表示せず、画像として表示するため、
                        // HyperLinkとしては扱わない
                        if (!hadMedia)
                        {
                            yield return factory(InlineContentType.Link, text ?? url, url);
                        }
                    }
                }
            }
        }

        public static IEnumerable<(InlineContentType Type, string Text)> MakeInlineTypedDataset(string raw)
        {
            var regex = new Regex(":(?<code>\\w*?):");

            var codes = regex.Matches(raw).OfType<Match>().Select(x => $":{x.Groups["code"].Value}:").ToList();

            foreach (var code in codes)
            {
                var index = raw.IndexOf(code);
                var headText = raw.Substring(0, index);
                if (headText.Length != 0)
                {
                    yield return (InlineContentType.Text, headText);
                }

                yield return (InlineContentType.Emoji, code);
                raw = raw.Substring(index + code.Length);
            }

            if (raw.Any())
            {
                yield return (InlineContentType.Text, raw);
            }
        }
    }
}
