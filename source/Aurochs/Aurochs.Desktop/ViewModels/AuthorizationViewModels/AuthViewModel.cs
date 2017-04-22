using Aurochs.Core.Entities;
using Aurochs.Desktop.ActionCreators;
using Aurochs.Linkage.Authentications;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aurochs.Desktop.ViewModels.AuthorizationViewModels
{
    public class AuthViewModel : BindableBase
    {
        public ICommand SignInCommand
        {
            get { return _SignInCommand ?? (_SignInCommand = new DelegateCommand(OnSignIn)); }
        }
        private ICommand _SignInCommand;

        public string InstanceUrl
        {
            get { return _InstanceUrl; }
            set { SetProperty(ref _InstanceUrl, value); }
        }
        private string _InstanceUrl;

        public string Email
        {
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
        }
        private string _Email;

        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        private string _Password;

        private void OnSignIn()
        {
            // 仮なのでベタ書き
            // Secureに書き直すこと
            AuthUtility.CreateAuthObjectWithPassword(InstanceUrl, Email, Password).
                ContinueWith(async x =>
                {
                    var auth = x.Result;
                    var reg = await AuthUtility.GetRegistration(InstanceUrl);

                    var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                    var path = Path.Combine(root, "Aurochs");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var authPath = Path.Combine(path, "auth.json");
                    var regPath = Path.Combine(path, "reg.json");

                    var jsonReg = JsonConvert.SerializeObject(reg);
                    using (var stream = new StreamWriter(regPath, false, Encoding.UTF8))
                    {
                        stream.Write(jsonReg);
                    }

                    var jsonAuth = JsonConvert.SerializeObject(auth);
                    using (var stream = new StreamWriter(authPath, false, Encoding.UTF8))
                    {
                        stream.Write(jsonAuth);
                    }

                    ApplicationActionCreator.StartApplication();
                });
        }
    }
}
