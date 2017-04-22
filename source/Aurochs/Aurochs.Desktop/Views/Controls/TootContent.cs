using Aurochs.Desktop.Views.Utility;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Aurochs.Desktop.Views.Controls
{
    public class TootContent : TextBlock
    {
        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }
        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), typeof(TootContent), new PropertyMetadata(Brushes.Black));

        public Brush UriForeground
        {
            get { return (Brush)GetValue(UriForegroundProperty); }
            set { SetValue(UriForegroundProperty, value); }
        }
        public static readonly DependencyProperty UriForegroundProperty =
            DependencyProperty.Register("UriForeground", typeof(Brush), typeof(TootContent), new PropertyMetadata(Brushes.Blue));

        public string Contents
        {
            get { return (string)GetValue(ContentsProperty); }
            set { SetValue(ContentsProperty, value); }
        }

        public static readonly DependencyProperty ContentsProperty =
            DependencyProperty.Register("Contents", typeof(string), typeof(TootContent), new PropertyMetadata(string.Empty, OnStatusChanged));

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TootContent toot)
            {
                if (toot.Contents is null)
                    return;

                if (toot is TextBlock textblock)
                {
                    textblock.Inlines.Clear();
                    foreach (var inline in GenerateInlines(toot, toot.Contents))
                    {
                        textblock.Inlines.Add(inline);
                    }
                }
            }
        }

        private static IEnumerable<Inline> GenerateInlines(TootContent parent, string status)
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

        private static (InlineContentType type, Inline inline) BuildInlines(InlineContentType type, string text, string url)
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
    }
}
