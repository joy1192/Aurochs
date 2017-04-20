using Aurochs.Core.Streams;
using Aurochs.Linkage.Authentications;
using Aurochs.Linkage.Streams;
using StatefulModel;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Flux.Core;
using Aurochs.Core.Messages;
using Aurochs.Desktop.ActionMessages;

namespace Aurochs.Desktop.ActionCreators
{
    public class TimelineActionCreator : MultipleDisposable
    {
        private ITimelineStream UserStream { get; set; }

        public void Initialize()
        {
            Auth.GetRegistration().
                ContinueWith(x =>
                {
                    var accessToken = Environment.GetEnvironmentVariable("AUROCHS_ACCESS_TOKEN", EnvironmentVariableTarget.User);
                    var stream = new TimelineStream(x.Result, accessToken);

                    this.Add(
                        stream.UserAsObservable().
                        OfType<UpdateStatusMessage>().
                        Subscribe(msg =>
                        {
                            Dispatcher.Default.Invoke(new UpdateStatusActionMessage(msg.Status, StatusSource.User));
                        }));
                    this.Add(
                        stream.UserAsObservable().
                        OfType<DeleteStatusMessage>().
                        Subscribe(msg =>
                        {
                            Dispatcher.Default.Invoke(new DeleteStatusActionMessage(msg.StatusId, StatusSource.User));
                        }));
                    this.Add(
                        stream.UserAsObservable().
                        OfType<NotificationMessage>().
                        Subscribe(msg =>
                        {
                        }));
                });
        }
    }
}
