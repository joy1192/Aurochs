using Aurochs.Core.Entities;
using Aurochs.Desktop.ActionCreators;
using Aurochs.Desktop.Events;
using Aurochs.Desktop.Helpers;
using Aurochs.Desktop.Models;
using Aurochs.Desktop.ViewModels.Contents;
using Aurochs.Desktop.Views.Utility;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Aurochs.Desktop.ViewModels
{
    public class PublicTimelineViewModel : TimelineViewModelBase
    {
        public int VisibleMaxCount { get; set; } = 100;
        public int MaxCount { get; set; } = 400;

        public string FilterInstanceName
        {
            get { return _FilterInstanceName; }
            set { SetProperty(ref _FilterInstanceName, value); }
        }
        private string _FilterInstanceName = "@mstdn.jp";

        public bool IsFilitered
        {
            get { return _IsFilitered; }
            set { SetProperty(ref _IsFilitered, value); }
        }
        private bool _IsFilitered;

        public ICommand ApplyFilterCommand
        {
            get { return _ApplyFilterCommand ?? (_ApplyFilterCommand = new DelegateCommand(OnApplyFilter)); }
        }
        private ICommand _ApplyFilterCommand;

        private void OnApplyFilter()
        {
            // TODO: 本来はStore経由
            DispatcherHelper.CurrentDispatcher.BeginInvoke((Action)(() =>
            {
                this.IsFilitered = true;

                UpdateStausCollectionVisibility();
            }));
        }

        public ICommand ReleaseFilterCommand
        {
            get { return _ReleaseFilterCommand ?? (_ReleaseFilterCommand = new DelegateCommand(OnReleaseFilter)); }
        }
        private ICommand _ReleaseFilterCommand;

        private void OnReleaseFilter()
        {
            // TODO: 本来はStore経由
            DispatcherHelper.CurrentDispatcher.BeginInvoke((Action)(() =>
            {
                this.IsFilitered = false;

                UpdateStausCollectionVisibility();
            }));
        }

        private void UpdateStausCollectionVisibility()
        {
            int visibleCount = 0;
            foreach (var status in this.StatusCollection)
            {
                if (visibleCount <= VisibleMaxCount)
                {
                    if (!this.IsFilitered)
                    {
                        status.Visibility = System.Windows.Visibility.Visible;
                        visibleCount++;
                    }
                    else
                    {
                        if (this.FilterInstanceName != status.InstanceName)
                        {
                            status.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            status.Visibility = System.Windows.Visibility.Visible;
                            visibleCount++;
                        }
                    }
                }
                else
                {
                    status.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

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

                            var item = new StatusViewModel(status);
                            this.StatusCollection.Insert(0, item);
                        }

                        while (removes.Count != 0)
                        {
                            var removeId = removes.Dequeue();
                            var removeTarget = this.StatusCollection.FirstOrDefault(x => x.StatusId == removeId);
                            this.StatusCollection.Remove(removeTarget);
                        }

                        while (MaxCount < this.StatusCollection.Count)
                        {
                            var tail = this.StatusCollection.Count - 1;
                            if (0 <= tail)
                            {
                                this.StatusCollection.RemoveAt(tail);
                            }
                        }

                        UpdateStausCollectionVisibility();
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
