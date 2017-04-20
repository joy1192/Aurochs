using Aurochs.Desktop.ActionCreators;
using Aurochs.Desktop.Events;
using Aurochs.Desktop.Helpers;
using Aurochs.Desktop.Models;
using Aurochs.Desktop.ViewModels.Contents;
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
    public class TimelineViewModel : BindableBase
    {
        public ObservableCollection<StatusViewModel> StatusCollection
        {
            get { return _StatusCollection; }
            set { SetProperty(ref _StatusCollection, value); }
        }
        private ObservableCollection<StatusViewModel> _StatusCollection = new ObservableCollection<StatusViewModel>();

        public TimelineViewModel()
        {
            var creator = new TimelineActionCreator();
            creator.Initialize();

            var store = StoreAccessor.Default.Get<PublicTimelineStore>();
            var observer = Observable.FromEventPattern<ApplicationLocalEventArgs>
            (
                handler => store.StoreContentChanged += handler,
                handler => store.StoreContentChanged -= handler
            ).
            Select(x => x.Sender).
            OfType<PublicTimelineStore>().
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

        public void Initalize()
        {
        }
    }
}
