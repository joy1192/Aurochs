using Aurochs.Desktop.ActionCreators;
using Aurochs.Desktop.ActionMessages;
using Aurochs.Desktop.Events;
using Infrastructure.Flux.Core;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Models
{
    [Export(typeof(IFluxStore))]
    public class ApplicationStore : MultipleDisposable, IFluxStore
    {
        private Dictionary<string, DispatcherToken> Tokens { get; } = new Dictionary<string, DispatcherToken>();

        public Dictionary<string, TimelineActionCreator> Timelines { get; } = new Dictionary<string, TimelineActionCreator>();

        [ImportingConstructor]
        public ApplicationStore()
        {
            this.Tokens[nameof(OnAuthenticated)] = Dispatcher.Default.Register<AuthenticatedMessage>(OnAuthenticated);
            this.Tokens[nameof(OnUnauthenticated)] = Dispatcher.Default.Register<UnauthenticatedMessage>(OnUnauthenticated);
            this.Tokens[nameof(OnWakeUpApplication)] = Dispatcher.Default.Register<WakeUpApplicationMessage>(OnWakeUpApplication);
            
            this.Add(() =>
            {
                foreach (var token in Tokens.Values)
                {
                    Dispatcher.Default.Release(token);
                }
            });
        }

        private void OnAuthenticated(AuthenticatedMessage msg)
        {
            var instance = msg.Registration.Instance;

            Timelines[instance] = new TimelineActionCreator(msg.Registration, msg.Auth);

            Authenticated?.Invoke(this, new ApplicationLocalEventArgs());
        }

        private void OnUnauthenticated(UnauthenticatedMessage msg)
        {
            Unauthenticated?.Invoke(this, new ApplicationLocalEventArgs());
        }

        private void OnWakeUpApplication(WakeUpApplicationMessage obj)
        {
            foreach (var timeline in Timelines)
            {
                timeline.Value.Initialize();
            }
        }

        public event EventHandler<ApplicationLocalEventArgs> Authenticated;

        public event EventHandler<ApplicationLocalEventArgs> Unauthenticated;
    }
}
