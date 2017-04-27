using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using SharpVectors.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Aurochs.Desktop.Views.Utility
{
    public class InlineGenerator
    {
        public static Inline CreateEmojiWithInlineUIContainer(Uri uri, Typeface typeface, double fontSize, double pixelPerDip)
        {
            var svg = new SvgDrawingCanvas();
            using (var reader = new FileSvgReader(new WpfDrawingSettings()) { SaveZaml = false })
            {
                var drawing = reader.Read(uri);
                svg.RenderDiagrams(drawing);
            }

            var sizer = new FormattedText("■", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Transparent, pixelPerDip);

            var vbox = new Viewbox()
            {
                Child = svg
            };
            vbox.Width = vbox.Height = sizer.Height;

            var uiContainer = new InlineUIContainer(vbox)
            {
                BaselineAlignment = System.Windows.BaselineAlignment.Center
            };
            return uiContainer;
        }
    }
}
