using Aurochs.Desktop.Helpers;
using Aurochs.Desktop.Views.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aurochs.Desktop.Views.Controls
{
    public class LazyImage : Image
    {
        public Uri UriSource
        {
            get { return (Uri)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }

        public static readonly DependencyProperty UriSourceProperty =
            DependencyProperty.Register("UriSource", typeof(Uri), typeof(LazyImage), new PropertyMetadata(null, OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LazyImage image)
            {
                image.UpdateImage();
            }
        }
        
        private void UpdateImage()
        {
            var source = this.UriSource;
            if (source == null)
                return;

            ImageLoader.RequestLoadImage(source, this);
        }
    }
}
