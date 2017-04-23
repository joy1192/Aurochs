using Aurochs.Core.Entities;
using Aurochs.Core.Messages;
using Aurochs.Core.Streams;
using Aurochs.Desktop.ActionMessages;
using Aurochs.Linkage.Authentications;
using Aurochs.Linkage.Streams;
using Infrastructure.Flux.Core;
using Newtonsoft.Json;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ActionCreators
{
    public class TimelineActionCreator : MultipleDisposable
    {
        public ApplicationRegistration Registration { get; }

        public Auth Auth { get; }

        private ITimelineStream _Stream { get; set; }

        public TimelineActionCreator(ApplicationRegistration registration, Auth auth)
        {
            this.Registration = registration;
            this.Auth = auth;
        }

        public void Initialize()
        {
            try
            {
                _Stream = new TimelineStream(Registration, Auth);

                this.Add(
                    _Stream.PublicAsObservable().
                    OfType<UpdateStatusMessage>().
                    Subscribe(msg =>
                    {
                        Dispatcher.Default.Invoke(new UpdateStatusActionMessage(msg.Status, StatusSource.Public));
                    }));
                this.Add(
                    _Stream.PublicAsObservable().
                    OfType<DeleteStatusMessage>().
                    Subscribe(msg =>
                    {
                        Dispatcher.Default.Invoke(new DeleteStatusActionMessage(msg.StatusId, StatusSource.Public));
                    }));


                this.Add(
                    _Stream.UserAsObservable().
                    OfType<UpdateStatusMessage>().
                    Subscribe(msg =>
                    {
                        Dispatcher.Default.Invoke(new UpdateStatusActionMessage(msg.Status, StatusSource.User));
                    }));
                this.Add(
                    _Stream.UserAsObservable().
                    OfType<DeleteStatusMessage>().
                    Subscribe(msg =>
                    {
                        Dispatcher.Default.Invoke(new DeleteStatusActionMessage(msg.StatusId, StatusSource.User));
                    }));
                this.Add(
                    _Stream.UserAsObservable().
                    OfType<NotificationMessage>().
                    Subscribe(msg =>
                    {
                    }));

                var userContents = _Stream.GetUserTimline().ToList();
                Dispatcher.Default.Invoke(new UserTimelineInitializeMessage() { TweetContent = userContents });

                var publicContents = _Stream.GetPublicTimline().ToList();
                Dispatcher.Default.Invoke(new PublicTimelineInitializeMessage() { TweetContent = publicContents });
            }
            catch (Exception)
            {
                // Initialize前の状態まで戻す
                foreach (var disposable in this)
                {
                    disposable?.Dispose();
                }
                this.Clear();

                // TODO: Exceptionの型により飛ばすエラーを切り替えること
                Dispatcher.Default.Invoke(new AuthenticationFailedMessage());
            }
        }
    }
}
