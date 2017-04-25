using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aurochs.Desktop.ViewModels.Contents
{
    public class AttachmentViewModel : BindableBase
    {
        public string Url
        {
            get { return _Url; }
            set { SetProperty(ref _Url, value); }
        }
        private string _Url;

        public ICommand OpenUrlCommand
        {
            get { return _OpenUrlCommand ?? (_OpenUrlCommand = new DelegateCommand(OnOpenUrl)); }
        }
        private ICommand _OpenUrlCommand;

        private void OnOpenUrl()
        {
            Process.Start(this.Url);
        }

        public AttachmentViewModel(string url)
        {
            this.Url = url;
        }
    }
}
