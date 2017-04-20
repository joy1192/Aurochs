using Aurochs.Linkage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Helpers
{
    public static class APIHelper
    {
        public static void CreateApp()
        {
            API.CreateApp(x => Process.Start(x));
        }
    }
}
