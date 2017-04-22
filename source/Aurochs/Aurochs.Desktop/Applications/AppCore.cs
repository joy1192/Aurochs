using Aurochs.Desktop.ActionCreators;
using Aurochs.Desktop.ActionMessages;
using Aurochs.Desktop.Events;
using Aurochs.Desktop.Models;
using Aurochs.Desktop.Views;
using Infrastructure.Flux.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aurochs.Desktop.Applications
{
    public class AppCore
    {
        private Window AuthWindow { get; set; }

        public AppCore(Application app)
        {
            var store = StoreAccessor.Default.Get<ApplicationStore>();

            var authed = Observable.FromEventPattern<ApplicationLocalEventArgs>(
               handler => store.Authenticated += handler,
               handler => store.Authenticated -= handler);

            var unauthed = Observable.FromEventPattern<ApplicationLocalEventArgs>(
               handler => store.Unauthenticated += handler,
               handler => store.Unauthenticated -= handler);

            authed.Select(x => x.Sender).
                OfType<ApplicationStore>().
                Subscribe(x =>
                {
                    app.Dispatcher.Invoke(() =>
                    {
                        app.MainWindow = new MainWindow();
                        app.MainWindow.Show();

                        AuthWindow?.Close();
                    });
                    ApplicationActionCreator.WakeUpApplication();
                });

            unauthed.Select(x => x.Sender).
                OfType<ApplicationStore>().
                Subscribe(x =>
                {
                    app.Dispatcher.Invoke(() =>
                    {
                        AuthWindow = new AuthorizationWindow();
                        AuthWindow.Show();
                    });
                });
        }

        public void Initialize()
        {
            ApplicationActionCreator.StartApplication();
        }
    }
}
