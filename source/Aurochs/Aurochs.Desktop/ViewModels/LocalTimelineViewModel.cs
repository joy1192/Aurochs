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
                        while (args.Adds.Count != 0)
                        {
                            var status = args.Adds.Dequeue();
                            this.StatusCollection.Insert(0, new StatusViewModel(status));
                        }

                        while (args.Removes.Count != 0)
                        {
                            var removeId = args.Removes.Dequeue();
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
