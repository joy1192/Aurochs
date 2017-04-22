using Aurochs.Core.Entities;
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

        public Queue<Status> Adds { get; set; } = new Queue<Status>();

        public Queue<long> Removes { get; set; } = new Queue<long>();
    }
}
