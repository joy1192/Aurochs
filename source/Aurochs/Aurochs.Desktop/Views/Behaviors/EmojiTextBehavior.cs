using Aurochs.Desktop.Views.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Aurochs.Desktop.Views.Behaviors
{
    public class EmojiTextBehavior : Behavior<TextBlock>
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EmojiTextBehavior), new PropertyMetadata(null, OnContentsChanged));

        private static void OnContentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EmojiTextBehavior behavior)
            {
                if (behavior.Text is null)
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

            if (this.Text is null)
                return;

            if (this.AssociatedObject is TextBlock textblock)
            {
                textblock.Inlines.Clear();
                Make(this, textblock);
            }
        }

        private static void Make(EmojiTextBehavior behavior, TextBlock textblock)
        {
            foreach (var inline in GenerateInlines(behavior.AssociatedObject, behavior.Text))
            {
                textblock.Inlines.Add(inline);
            }
        }

        private static IEnumerable<Inline> GenerateInlines(TextBlock textblock, string text)
        {
            foreach (var item in InlineResolver.MakeInlineTypedDataset(text))
            {
                // TextBlockに設定された見た目を反映
                switch (item.Type)
                {
                    case InlineContentType.Text:
                        {
                            var run = new Run(item.Text)
                            {
                                Foreground = textblock.Foreground,
                                FontFamily = textblock.FontFamily,
                                FontSize = textblock.FontSize,
                                FontStretch = textblock.FontStretch,
                                FontWeight = textblock.FontWeight,
                                FontStyle = textblock.FontStyle
                            };
                            yield return run;
                        }
                        break;
                    case InlineContentType.Emoji:
                        {
                            var resolver = EmojiUriResolver.Resolver;
                            var uri = resolver.CodeToUrl(item.Text);
                            var uiContainer = InlineGenerator.CreateEmojiWithInlineUIContainer(uri, new Typeface(textblock.FontFamily, textblock.FontStyle, textblock.FontWeight, textblock.FontStretch), textblock.FontSize, VisualTreeHelper.GetDpi(textblock).PixelsPerDip);
                            if (uiContainer != null)
                            {
                                yield return uiContainer;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
