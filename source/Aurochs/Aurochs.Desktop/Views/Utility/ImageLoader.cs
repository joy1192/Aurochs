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
    public class LoadingRequest
    {
        public string URI { get; internal set; }
        public Action Callback { get; internal set; }
    }

    public static class ImageLoader
    {
        private const int LoadingThreadSize = 3;

        private const int MaxCacheCount = 1000;

        private static List<Thread> Threads { get; } = new List<Thread>();

        private static ConcurrentQueue<LoadingRequest> Requests { get; } = new ConcurrentQueue<LoadingRequest>();

        private static ConcurrentDictionary<string, ImageSource> LoadedImages { get; } = new ConcurrentDictionary<string, ImageSource>();

        private static ConcurrentQueue<string> LoadedImageUris { get; } = new ConcurrentQueue<string>();

        static ImageLoader()
        {
            for (int i = 0; i < LoadingThreadSize; i++)
            {
                Threads.Add(MakeLoadingThread());
            }
        }

        private static Thread MakeLoadingThread()
        {
            var thread = new Thread(new ThreadStart(Load))
            {
                IsBackground = true
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return thread;
        }

        public static void RequestLoadImage(string uri, Action callback)
        {
            Requests.Enqueue(new LoadingRequest() { URI = uri, Callback = callback });
        }

        public static bool TryGetImage(string uri, out ImageSource image)
        {
            return LoadedImages.TryGetValue(uri, out image);
        }

        private static void Load()
        {
            while (true)
            {
                if (Requests.TryDequeue(out LoadingRequest request))
                {
                    try
                    {
                        var url = request.URI;
                        using (var web = new HttpClient())
                        {
                            var bytes = web.GetByteArrayAsync(url).Result;
                            using (var stream = new MemoryStream(bytes))
                            {
                                var bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = stream;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                bitmap.Freeze();

                                if (LoadedImages.TryAdd(request.URI, bitmap))
                                {
                                    LoadedImageUris.Enqueue(request.URI);

                                    // ハンドラ実行
                                    request.Callback?.Invoke();

                                    while (MaxCacheCount < LoadedImageUris.Count)
                                    {
                                        if (LoadedImageUris.TryDequeue(out string result))
                                        {
                                            if (LoadedImages.TryRemove(result, out ImageSource value))
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.TraceWarning(e.ToString());
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
