using Aurochs.Desktop.ViewModels.Contents;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ViewModels
{
    public class TimelineViewModelBase : BindableBase
    {
        public ObservableCollection<StatusViewModel> StatusCollection
        {
            get { return _StatusCollection; }
            set { SetProperty(ref _StatusCollection, value); }
        }
        private ObservableCollection<StatusViewModel> _StatusCollection = new ObservableCollection<StatusViewModel>();
    }
}
