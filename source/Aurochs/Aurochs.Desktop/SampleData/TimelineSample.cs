using Aurochs.Core.Entities;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.SampleData
{
    public class TimelineSample : BindableBase
    {
        public ObservableCollection<StatusSample> Toots
        {
            get { return _Tweets; }
            set { SetProperty(ref _Tweets, value); }
        }
        private ObservableCollection<StatusSample> _Tweets = new ObservableCollection<StatusSample>();


        public TimelineSample()
        {
        }
    }
}
