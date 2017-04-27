using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aurochs.Desktop.ViewModels.Contents;
using System.IO;
using System.Linq;
using Aurochs.Desktop.Views.Utility;

namespace Aurochs.Desktop.Test
{
    public abstract class InlineMock
    {
    }

    public class RunMock : InlineMock
    {
        public string Text { get; set; }
    }

    public class HyperlinkMock : InlineMock
    {
        public string Text { get; set; }

        public string LinkUrl { get; set; }
    }

    [TestClass]
    public class ParseLogic
    {
        [TestMethod]
        public void ResolveTootContent()
        {
            var raw = File.ReadAllText("./RawToot.txt");
            var inlines = InlineResolver.Resolve(raw, Enumerable.Empty<Uri>(), BuildInlineMock).ToArray();

            {
                var inline = inlines[0].Inline as HyperlinkMock;
                Assert.IsNotNull(inline);
                Assert.AreEqual(inline.Text, "@mzne");
                Assert.AreEqual(inline.LinkUrl, "https://friends.nico/@mzne");
            }
            {
                var inline = inlines[1].Inline as RunMock;
                Assert.IsNotNull(inline);
                Assert.AreEqual(inline.Text, " 国語力の足りない業界という闇");
            }
            {
                var inline = inlines[2].Inline as RunMock;
                Assert.IsNotNull(inline);
                Assert.AreEqual(inline.Text, "\n");
            }
            {
                var inline = inlines[3].Inline as HyperlinkMock;
                Assert.IsNotNull(inline);
                Assert.AreEqual(inline.Text, "@joy");
                Assert.AreEqual(inline.LinkUrl, "https://friends.nico/@joy");
            }
            {
                var inline = inlines[4].Inline as RunMock;
                Assert.IsNotNull(inline);
                Assert.AreEqual(inline.Text, " (Test Tootもついでに)");
            }
            {
                var inline = inlines[5].Inline as RunMock;
                Assert.IsNotNull(inline);
                Assert.AreEqual(inline.Text, "\n");
            }
            {
                var inline = inlines[6].Inline as HyperlinkMock;
                Assert.IsNotNull(inline);
                Assert.AreEqual(inline.Text, "friends.nico/");
                Assert.AreEqual(inline.LinkUrl, "https://friends.nico/");
            }
        }

        private (InlineContentType Type, InlineMock inline) BuildInlineMock(InlineContentType type, string text, string url)
        {
            switch (type)
            {
                case InlineContentType.Text:
                    return (type, new RunMock() { Text = text });
                case InlineContentType.Link:
                    return (type, new HyperlinkMock() { Text = text, LinkUrl = url });
                case InlineContentType.Mension:
                    return (type, new HyperlinkMock() { Text = text, LinkUrl = url });
                case InlineContentType.HashTag:
                    return (type, new HyperlinkMock() { Text = text, LinkUrl = url });
                default:
                    Assert.Fail();
                    throw new Exception();
            }
        }
    }
}
