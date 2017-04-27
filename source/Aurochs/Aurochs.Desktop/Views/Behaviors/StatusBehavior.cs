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
            DependencyProperty.Register("Contents", typeof(string), typeof(StatusBehavior), new PropertyMetadata(string.Empty, OnContentsChanged));

        public IEnumerable<Uri> MediaUris
        {
            get { return (IEnumerable<Uri>)GetValue(MediaUrisProperty); }
            set { SetValue(MediaUrisProperty, value); }
        }

        public static readonly DependencyProperty MediaUrisProperty =
            DependencyProperty.Register("MediaUris", typeof(IEnumerable<Uri>), typeof(StatusBehavior), new PropertyMetadata(Enumerable.Empty<Uri>(), OnContentsChanged));

        private static void OnContentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StatusBehavior behavior)
            {
                if (behavior.Contents is null)
                    return;

                if (behavior.AssociatedObject is TextBlock textblock)
                {
                    textblock.Inlines.Clear();
                    Make(behavior, textblock);
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.Contents is null)
                return;

            if (this.AssociatedObject is TextBlock textblock)
            {
                textblock.Inlines.Clear();
                Make(this, textblock);
            }
        }

        private static void Make(StatusBehavior behavior, TextBlock textblock)
        {
            foreach (var inline in GenerateInlines(behavior, behavior.Contents, behavior.MediaUris))
            {
                textblock.Inlines.Add(inline);
            }
        }

        private static IEnumerable<Inline> GenerateInlines(StatusBehavior parent, string content, IEnumerable<Uri> uris)
        {
            foreach (var item in InlineGenerator.Resolve<Inline>(content, uris, BuildInlines))
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
                case InlineContentType.Emoji:
                    {
                        var icon = Application.Current.Resources["nicoru"] as Image;
                        UIElement element = new Border()
                        {
                            Width = 15,
                            Height = 15,
                            Background = new VisualBrush(icon)
                        };
                        var uiContainer = new InlineUIContainer(element)
                        {
                            BaselineAlignment = System.Windows.BaselineAlignment.Baseline
                        };
                        return (type, uiContainer);
                    }
                default:
                    throw new ArgumentException("invalid arguments", nameof(type));
            }
        }
    }
}
