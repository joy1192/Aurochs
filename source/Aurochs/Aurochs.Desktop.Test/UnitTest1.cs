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
            var inlines = InlineGenerator.Resolve(raw, BuildInlineMock).ToArray();

            Assert.IsInstanceOfType(inlines[0], typeof(HyperlinkMock));
            {
                var inline = inlines[0].Inline as HyperlinkMock;
                Assert.AreEqual(inline.Text, "@mzne");
                Assert.AreEqual(inline.Text, "https://friends.nico/@mzne");
            }
            Assert.IsInstanceOfType(inlines[1], typeof(RunMock));
            {
                var inline = inlines[1].Inline as RunMock;
                Assert.AreEqual(inline.Text, " 国語力の足りない業界という闇\n");
            }
            Assert.IsInstanceOfType(inlines[2], typeof(HyperlinkMock));
            {
                var inline = inlines[2].Inline as HyperlinkMock;
                Assert.AreEqual(inline.Text, "@joy");
                Assert.AreEqual(inline.Text, "https://friends.nico/@joy");
            }
            Assert.IsInstanceOfType(inlines[3], typeof(RunMock));
            {
                var inline = inlines[3].Inline as RunMock;
                Assert.AreEqual(inline.Text, " (Test Tootもついでに)\n");
            }
            Assert.IsInstanceOfType(inlines[4], typeof(HyperlinkMock));
            {
                var inline = inlines[4].Inline as HyperlinkMock;
                Assert.AreEqual(inline.Text, "friends.nico/");
                Assert.AreEqual(inline.Text, "https://friends.nico/");
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
