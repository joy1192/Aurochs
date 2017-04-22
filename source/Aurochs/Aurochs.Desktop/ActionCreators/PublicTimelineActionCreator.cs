using Aurochs.Core.Messages;
using Aurochs.Core.Streams;
using Aurochs.Desktop.ActionMessages;
using Aurochs.Linkage.Authentications;
using Aurochs.Linkage.Streams;
using Infrastructure.Flux.Core;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ActionCreators
{
    public class PublicTimelineActionCreator : MultipleDisposable
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
                        stream.PublicAsObservable().
                        OfType<UpdateStatusMessage>().
                        Subscribe(msg =>
                        {
                            Dispatcher.Default.Invoke(new UpdateStatusActionMessage(msg.Status, StatusSource.Public));
                        }));
                    this.Add(
                        stream.PublicAsObservable().
                        OfType<DeleteStatusMessage>().
                        Subscribe(msg =>
                        {
                            Dispatcher.Default.Invoke(new DeleteStatusActionMessage(msg.StatusId, StatusSource.Public));
                        }));
                });
        }
    }
}
