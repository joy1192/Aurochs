using Aurochs.Core.Entities;
using Aurochs.Desktop.ActionCreators;
using Aurochs.Desktop.Events;
using Aurochs.Desktop.Helpers;
using Aurochs.Desktop.Models;
using Aurochs.Desktop.ViewModels.Contents;
using Aurochs.Desktop.Views.Utility;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ViewModels
{
    public class PublicTimelineViewModel : TimelineViewModelBase
    {
        public PublicTimelineViewModel()
        {
            var store = StoreAccessor.Default.Get<PublicTimelineStore>();
            var observer = Observable.FromEventPattern<ApplicationLocalEventArgs>
            (
                handler => store.StoreContentChanged += handler,
                handler => store.StoreContentChanged -= handler
            ).
            Where(x => x.Sender is PublicTimelineStore).
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
