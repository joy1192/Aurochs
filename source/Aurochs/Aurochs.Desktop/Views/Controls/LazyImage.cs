using Aurochs.Desktop.Helpers;
using Aurochs.Desktop.Views.Utility;
using System;
using System.Collections.Generic;
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
            DependencyProperty.Register("LoadSource", typeof(string), typeof(LazyImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnSourceChanged));

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
                DispatcherHelper.CurrentDispatcher.BeginInvoke((Action)(() =>
                {
                    image.Source = imageSource;
                }));
            }
            else
            {
                ImageLoader.RequestLoadImage(source,
                    () =>
                    {
                        DispatcherHelper.CurrentDispatcher.BeginInvoke((Action)(() =>
                        {
                            // Binding等により既に依頼した時点とはLoadSourceが変わっていたら、
                            // 無視する
                            if (image.LoadSource != source)
                                return;

                            if (ImageLoader.TryGetImage(source, out ImageSource loadedBitmap))
                            {
                                image.Source = imageSource;
                            }
                        }));
                    });
            }
        }
    }
}
