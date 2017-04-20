using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Events
{
    public class TimelineContentUpdatedEventArgs : ApplicationLocalEventArgs
    {
        public static TimelineContentUpdatedEventArgs Default { get; } = new TimelineContentUpdatedEventArgs();
    }
}
