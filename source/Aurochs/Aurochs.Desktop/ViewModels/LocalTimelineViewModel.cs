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
            var creator = new TimelineActionCreator();
            creator.Initialize();

            var store = StoreAccessor.Default.Get<LocalTimelineStore>();
            var observer = Observable.FromEventPattern<ApplicationLocalEventArgs>
            (
                handler => store.StoreContentChanged += handler,
                handler => store.StoreContentChanged -= handler
            ).
            Select(x => x.Sender).
            OfType<LocalTimelineStore>().
            Subscribe(s =>
            {
                DispatcherHelper.CurrentDispatcher.Invoke(() =>
                {
                    try
                    {
                        while (this.StatusCollection.Count < s.StatusCollection.Count)
                        {
                            this.StatusCollection.Add(new StatusViewModel());
                        }
                        while (this.StatusCollection.Count > s.StatusCollection.Count)
                        {
                            this.StatusCollection.RemoveAt(0);
                        }
                        for (int i = 0; i < this.StatusCollection.Count; i++)
                        {
                            this.StatusCollection[i].Update(s.StatusCollection[i]);
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
