using Aurochs.Desktop.Views.Utility;
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
    public class StatusBehavior : Behavior<TextBlock>
    {
        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }
        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), typeof(StatusBehavior), new PropertyMetadata(Brushes.Black));

        public Brush UriForeground
        {
            get { return (Brush)GetValue(UriForegroundProperty); }
            set { SetValue(UriForegroundProperty, value); }
        }
        public static readonly DependencyProperty UriForegroundProperty =
            DependencyProperty.Register("UriForeground", typeof(Brush), typeof(StatusBehavior), new PropertyMetadata(Brushes.Blue));

        public string Contents
        {
            get { return (string)GetValue(ContentsProperty); }
            set { SetValue(ContentsProperty, value); }
        }

        public static readonly DependencyProperty ContentsProperty =
            DependencyProperty.Register("Contents", typeof(string), typeof(StatusBehavior), new PropertyMetadata(string.Empty, OnStatusChanged));

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StatusBehavior behavior)
            {
                if (behavior.Contents is null)
                    return;

                if (behavior.AssociatedObject is TextBlock textblock)
                {
                    textblock.Inlines.Clear();
                    foreach (var inline in GenerateInlines(behavior, behavior.Contents))
                    {
                        textblock.Inlines.Add(inline);
                    }
                }
            }
        }

        private static IEnumerable<Inline> GenerateInlines(StatusBehavior parent, string status)
        {
            foreach (var item in InlineGenerator.Resolve<Inline>(status, BuildInlines))
            {
                // TextBlockに設定された見た目を反映
                switch (item.Type)
                {
                    case InlineContentType.Text:
                        item.Inline.Foreground = parent.TextForeground;
                        break;
                    case InlineContentType.Link:
                    case InlineContentType.Mension:
                    case InlineContentType.HashTag:
                        item.Inline.Foreground = parent.UriForeground;
                        break;
                    default:
                        break;
                }
                yield return item.Inline;
            }
        }

        private static (InlineContentType type,Inline inline) BuildInlines(InlineContentType type, string text, string url)
        {
            switch (type)
            {
                case InlineContentType.Text:
                    {
                        return (type, new Run(text));
                    }
                case InlineContentType.Link:
                case InlineContentType.Mension:
                case InlineContentType.HashTag:
                    {
                        var hyperlink = new Hyperlink()
                        {
                            Focusable = false
                        };
                        hyperlink.Inlines.Add(text);
                        hyperlink.Command = new DelegateCommand<string>(URL =>
                        {
                            System.Diagnostics.Process.Start(URL);
                        });
                        hyperlink.CommandParameter = url;
                        return (type, hyperlink);
                    }
                default:
                    throw new ArgumentException("invalid arguments", nameof(type));
            }
        }
        
        private void Resolve_()
        {
            //string linkPattern = @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9@\-\.\?\,\'\/\\\+&amp;%\$#_]*)?";
            //var linkMatchCollection = Regex.Matches(status, linkPattern).OfType<Match>().ToList();

            //var linkWorkCollcetion = linkMatchCollection.Select(match => new HHH
            //{
            //    Type = ContentType.Link,
            //    Head = status.IndexOf(match.Value),
            //    Length = match.Length,
            //    Content = match.Value
            //}).
            //ToList();

            //string mentionPattern = "@[a-zA-Z0-9]*@*[a-zA-Z0-9\\.]*";
            //var mentionMatchCollection = Regex.Matches(status, mentionPattern).OfType<Match>().ToList();
            //var mentionWorkCollcetion = mentionMatchCollection.Select(match => new HHH
            //{
            //    Type = ContentType.Mension,
            //    Head = status.IndexOf(match.Value),
            //    Length = match.Length,
            //    Content = match.Value
            //}).
            //ToList();

            //// URLの一部に@xxxが含まれている場合を考慮し、重複範囲を掃除する
            //foreach (var link in linkWorkCollcetion)
            //{
            //    var linkHead = link.Head;
            //    var linkTail = (link.Head + link.Length);
            //    mentionWorkCollcetion.RemoveAll(x => linkHead <= x.Head && (x.Head + x.Length) <= linkTail);
            //}

            //// 連結して先頭Index順にソート
            //var collection = linkWorkCollcetion.
            //    Concat(mentionWorkCollcetion).
            //    OrderBy(x => x.Head).
            //    ToList();

            //int head = 0;
            //foreach (var item in collection)
            //{
            //    // 先頭がhttp(s)で始まっていなかった場合、
            //    if (item.Head != head)
            //    {
            //        // 文頭からhttp(s)の先頭までテキストとして解釈
            //        var text = status.Substring(head, item.Head - head);
            //        yield return (ContentType.Text, text);
            //    }

            //    if (item.Type == ContentType.Link)
            //    {
            //        // http(s)をLinkとして解釈
            //        var link = status.Substring(item.Head, item.Length);
            //        yield return (ContentType.Link, link);

            //        // http(s)の末尾まで読み込んだので、次の読み始め位置を更新
            //        head = item.Head + item.Length;
            //    }
            //    else if (item.Type == ContentType.Mension)
            //    {
            //        // @xxxをMentionとして解釈
            //        var mention = status.Substring(item.Head, item.Length);
            //        yield return (ContentType.Mension, mention);

            //        // @xxxの末尾まで読み込んだので、次の読み始め位置を更新
            //        head = item.Head + item.Length;
            //    }
            //}

            //var tailText = status.Substring(head);
            //if (!string.IsNullOrEmpty(tailText))
            //{
            //    yield return (ContentType.Text, tailText);
            //}
        }
    }
}
