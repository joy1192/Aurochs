using Aurochs.Desktop.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aurochs.Desktop.ViewModels
{
    public class PostAreaViewModel : BindableBase
    {
        public ICommand TootCommand
        {
            get { return _TootCommand ?? (_TootCommand = new DelegateCommand(OnToot)); }
        }
        private ICommand _TootCommand;

        public string Text
        {
            get { return _Text; }
            set { SetProperty(ref _Text, value); }
        }
        private string _Text;

        private void OnToot()
        {
            // TODO: MUST: Fluxのアクセス順序を明らかに逸脱。絶対問題になるので、Creator部分を再設計すること。
            var store = StoreAccessor.Default.Get<ApplicationStore>();

            store.Timelines["friends.nico"].Post(Text);
            Text = null;

        }
    }
}
