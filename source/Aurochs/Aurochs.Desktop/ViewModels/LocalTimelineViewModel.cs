using Aurochs.Core.Entities;
using Aurochs.Desktop.ActionCreators;
using Aurochs.Desktop.Events;
using Aurochs.Desktop.Helpers;
using Aurochs.Desktop.Models;
using Aurochs.Desktop.ViewModels.Contents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ViewModels
{
    public class LocalTimelineViewModel : TimelineViewModelBase
    {
        public LocalTimelineViewModel()
        {
            var store = StoreAccessor.Default.Get<LocalTimelineStore>();
            var observer = Observable.FromEventPattern<ApplicationLocalEventArgs>
            (
                handler => store.StoreContentChanged += handler,
                handler => store.StoreContentChanged -= handler
            ).
            Where(x => x.Sender is LocalTimelineStore).
            Select(x => x.EventArgs).
            OfType<TimelineContentUpdatedEventArgs>().
            Subscribe(args =>
            {
                DispatcherHelper.CurrentDispatcher.Invoke(() =>
                {
                    try
                    {
                        var adds = new Queue<Status>();
                        foreach (var add in args.Adds)
                        {
                            adds.Enqueue(add);
                        }
                        var removes = new Queue<long>();
                        foreach (var remove in args.Removes)
                        {
                            removes.Enqueue(remove);
                        }

                        while (adds.Count != 0)
                        {
                            var status = adds.Dequeue();
                            this.StatusCollection.Insert(0, new StatusViewModel(status));
                        }

                        while (removes.Count != 0)
                        {
                            var removeId = removes.Dequeue();
                            var removeTarget = this.StatusCollection.FirstOrDefault(x => x.StatusId == removeId);
                            this.StatusCollection.Remove(removeTarget);
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                    }
                });
            });
        }
    }
}
