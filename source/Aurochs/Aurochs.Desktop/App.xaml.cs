using Aurochs.Desktop.Applications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Aurochs.Desktop
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private AppCore _Core;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Trace.Listeners.Remove("Default");

            var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(root, "Aurochs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var logPath = Path.Combine(path, "logs");
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            var filename = $"Aurochs_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log";
            var logfilePath = Path.Combine(logPath, filename);

            Trace.Listeners.Add(new TextWriterTraceListener(logfilePath));

            this.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _Core = new AppCore(this);
            _Core.Initialize();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.TraceError(e.ToString());
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Trace.TraceError(e.ToString());
        }
    }
}
