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
            var creator = new PublicTimelineActionCreator();
            creator.Initialize();

            // TODO: 本来の位置は初期化Message受けたModel側
            InstanceMetadata.InstanceName = "friends.nico";

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
