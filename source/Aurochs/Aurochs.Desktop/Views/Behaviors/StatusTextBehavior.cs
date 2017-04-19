using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Aurochs.Desktop.Views.Behaviors
{
    public class StatusTextBehavior : Behavior<TextBlock>
    {
        enum ContentType
        {
            Text,
            Link,
            Mension,
            HashTag,
        }

        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }
        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), typeof(StatusTextBehavior), new PropertyMetadata(Brushes.Black));

        public Brush UriForeground
        {
            get { return (Brush)GetValue(UriForegroundProperty); }
            set { SetValue(UriForegroundProperty, value); }
        }
        public static readonly DependencyProperty UriForegroundProperty =
            DependencyProperty.Register("UriForeground", typeof(Brush), typeof(StatusTextBehavior), new PropertyMetadata(Brushes.Blue));

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(StatusTextBehavior), new PropertyMetadata(string.Empty, OnStatusChanged));

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StatusTextBehavior behavior)
            {
                if (behavior.Status is null)
                    return;

                if (behavior.AssociatedObject is TextBlock textblock)
                {
                    textblock.Inlines.Clear();
                    foreach (var inline in GenerateInlines(behavior, behavior.Status))
                    {
                        textblock.Inlines.Add(inline);
                    }
                }
            }
        }

        private static IEnumerable<Inline> GenerateInlines(StatusTextBehavior parent, string status)
        {
            foreach (var item in Resolve(status))
            {
                switch (item.Type)
                {
                    case ContentType.Text:
                        yield return GenerateTextInline(parent, item.Text);
                        break;
                    case ContentType.Link:
                        yield return GenerateUriInline(parent, item.Text);
                        break;
                    case ContentType.Mension:
                        throw new NotImplementedException();
                    case ContentType.HashTag:
                        throw new NotImplementedException();
                    default:
                        break;
                }
            }
        }

        private static Inline GenerateUriInline(StatusTextBehavior parent, string text)
        {
            var hyperlink = new Hyperlink()
            {
                Focusable = false
            };
            hyperlink.Inlines.Add(text);
            hyperlink.Foreground = parent.UriForeground;
            hyperlink.Command = new DelegateCommand<string>(uri =>
            {
                System.Diagnostics.Process.Start(uri);
            });
            hyperlink.CommandParameter = text;
            return hyperlink;
        }

        private static Inline GenerateTextInline(StatusTextBehavior parent, string text)
        {
            return new Run(text)
            {
                Focusable = false,
                Foreground = parent.TextForeground
            };
        }

        private static IEnumerable<(ContentType Type, string Text)> Resolve(string status)
        {
            string pattern = @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?";
            var matchCollection = Regex.Matches(status, pattern).OfType<Match>().ToList();

            var collcetion = matchCollection.Select(match => new
            {
                Head = status.IndexOf(match.Value),
                Length = match.Length,
                Content = match.Value
            }).
            OrderBy(x => x.Head).
            ToList();

            int head = 0;
            foreach (var item in collcetion)
            {
                // 先頭がhttp(s)で始まっていなかった場合、
                if (item.Head != head)
                {
                    // 文頭からhttp(s)の先頭までテキストとして解釈
                    var text = status.Substring(head, item.Head - head);
                    yield return (ContentType.Text, text);
                }

                // http(s)をLinkとして解釈
                var link = status.Substring(item.Head, item.Length);
                yield return (ContentType.Link, link);

                // http(s)の末尾まで読み込んだので、次の読み始め位置を更新
                head = item.Head + item.Length;
            }

            var tailText = status.Substring(head);
            if (!string.IsNullOrEmpty(tailText))
            {
                yield return (ContentType.Text, tailText);
            }
        }
    }
}
