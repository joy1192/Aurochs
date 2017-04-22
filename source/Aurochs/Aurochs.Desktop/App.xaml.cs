using Aurochs.Desktop.Applications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

            _Core = new AppCore(this);
            _Core.Initialize();
        }
    }
}
