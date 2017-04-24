using Aurochs.Desktop.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aurochs.Desktop.Views.Utility
{
    public class Request
    {
        public Uri Uri { get; internal set; }

        public byte[] RawImage { get; internal set; }

        public Image Image { get; internal set; }
    }

    public static class ImageLoader
    {
        private const int LoadThreadSize = 6;

        private const int DecodeThreadSize = 1;

        private const int MaxCacheCount = 1000;

        private const int RetryCount = 3;

        private static List<Thread> Threads { get; } = new List<Thread>();

        private static ConcurrentQueue<Request> LoadRequests { get; } = new ConcurrentQueue<Request>();

        private static ConcurrentQueue<Request> DecodeRequests { get; } = new ConcurrentQueue<Request>();

        private static ConcurrentDictionary<Uri, ImageSource> LoadedImages { get; } = new ConcurrentDictionary<Uri, ImageSource>();

        private static ConcurrentQueue<Uri> LoadedImageUris { get; } = new ConcurrentQueue<Uri>();

        private static ConcurrentDictionary<Uri, ImageSource> PersistentImages { get; } = new ConcurrentDictionary<Uri, ImageSource>();

        static ImageLoader()
        {
            for (int i = 0; i < LoadThreadSize; i++)
            {
                Threads.Add(MakeLoadingThread());
            }

            for (int i = 0; i < DecodeThreadSize; i++)
            {
                Threads.Add(MakeDecodingThread());
            }
        }

        private static Thread MakeLoadingThread()
        {
            var thread = new Thread(new ThreadStart(Load))
            {
                IsBackground = true
            };
            thread.Start();
            return thread;
        }

        private static Thread MakeDecodingThread()
        {
            var thread = new Thread(new ThreadStart(Decode))
            {
                IsBackground = true
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return thread;
        }

        public static void RequestLoadImage(Uri uri, Image image)
        {
            LoadRequests.Enqueue(new Request() { Uri = uri, Image = image });
        }

        private static bool TryGetImage(Uri uri, out ImageSource image)
        {
            if (PersistentImages.TryGetValue(uri, out image))
                return true;

            return LoadedImages.TryGetValue(uri, out image);
        }

        private static void Load()
        {
            while (true)
            {
                if (LoadRequests.TryDequeue(out Request request))
                {
                    LoadBytes(request, RetryCount).Wait();
                }
                else
                {
                    Thread.Sleep(30);
                }
            }
        }

        private static async Task LoadBytes(Request request, int retry)
        {
            while (0 < retry--)
            {
                try
                {
                    using (var client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(1000) })
                    {
                        var source = request.Uri;
                        var response = await client.GetAsync(source).ConfigureAwait(false);
                        var result = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                        request.RawImage = result;
                        DecodeRequests.Enqueue(request);
                    }
                    break;
                }
                catch (Exception e)
                {
                    Trace.TraceWarning($"{e}");
                    continue;
                }
            }
        }

        private static void Decode()
        {
            while (true)
            {
                if (DecodeRequests.TryDequeue(out Request request))
                {
                    var bitmap = CreateImage(request.RawImage, 60, 60);

                    if (LoadedImages.TryAdd(request.Uri, bitmap))
                    {
                        LoadedImageUris.Enqueue(request.Uri);

                        DispatcherHelper.CurrentDispatcher.BeginInvoke((Action)(() =>
                        {
                            var UI = request.Image;
                            UI.Source = bitmap;
                        }));

                        // Cache上限を超えたイメージを破棄
                        while (MaxCacheCount < LoadedImageUris.Count)
                        {
                            if (LoadedImageUris.TryDequeue(out Uri result))
                            {
                                if (LoadedImages.TryRemove(result, out ImageSource value))
                                {
                                    // some log
                                    Trace.TraceInformation("remove max cap");
                                }
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(30);
                    }
                }
            }
        }

        private static BitmapImage CreateImage(byte[] bytes, int dpw, int dph)
        {
            try
            {
                using (var ms = new MemoryStream(bytes, false))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    if (dpw > 0 || dph > 0)
                    {
                        bitmap.DecodePixelWidth = dpw;
                        bitmap.DecodePixelHeight = dph;
                    }
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
