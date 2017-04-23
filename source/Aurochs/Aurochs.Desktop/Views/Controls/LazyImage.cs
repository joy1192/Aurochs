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
        public string LoadSource
        {
            get { return (string)GetValue(LoadSourceProperty); }
            set { SetValue(LoadSourceProperty, value); }
        }

        public static readonly DependencyProperty LoadSourceProperty =
            DependencyProperty.Register("LoadSource", typeof(string), typeof(LazyImage), new FrameworkPropertyMetadata(null, OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LazyImage image)
            {
                LoadImage(image);
            }
        }
        
        private static void LoadImage(LazyImage image)
        {
            image.Source = null;

            var source = image.LoadSource;
            if (source == null)
                return;

            if (ImageLoader.TryGetImage(source, out ImageSource imageSource))
            {
                image.SetSource(source, imageSource);
            }
            else
            {
                ImageLoader.RequestLoadImage(source, lazySource => image.SetSource(source, lazySource));
            }
        }


        private void SetSource(string requestUrl, ImageSource source)
        {
            if(source == null)
            {
                Trace.TraceWarning("source is null");
            }

            DispatcherHelper.CurrentDispatcher.BeginInvoke((Action)(() =>
            {
                // Binding等により既に依頼した時点とはLoadSourceが変わっていたら、
                // 無視する
                if (this.LoadSource != requestUrl)
                    return;

                this.Source = source;
            }));
        }
    }
}
