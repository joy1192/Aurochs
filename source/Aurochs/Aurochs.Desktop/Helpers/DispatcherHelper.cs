using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Aurochs.Desktop.Helpers
{
    public static class DispatcherHelper
    {
        public static Dispatcher CurrentDispatcher { get; set; }
    }
}
