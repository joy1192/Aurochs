using Aurochs.Core.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using Aurochs.Core.Messages;
using Mastonet;
using Mastonet.Entities;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Aurochs.Linkage.Converter;
using Aurochs.Core.Entities;
using System.Linq;

namespace Aurochs.Linkage.Streams
{
    public class TimelineStream : ITimelineStream
    {
        private AppRegistration _AppRegistration { get; set; }

        private MastodonClient _Client { get; set; }

        private TimelineStreaming _Steaming { get; set; }

        private Mastonet.Entities.Auth _Auth { get; set; }

        public TimelineStream(ApplicationRegistration appRegistration, Core.Entities.Auth auth)
        {
            this._AppRegistration = appRegistration.ToMastonetAppRegistration();
            this._Auth = auth.ToMastonetAuth();

            _Client = new MastodonClient(this._AppRegistration, this._Auth);
            _Steaming = _Client.GetUserStreaming();
        }

        public IObservable<StreamingMessage> UserAsObservable()
        {
            return new UserStreamingObservable(this._AppRegistration, this._Auth);
        }

        public IObservable<StreamingMessage> PublicAsObservable()
        {
            return new PublicStreamingObservable(_AppRegistration, this._Auth);
        }

        public IEnumerable<Core.Entities.Status> GetUserTimline()
        {
            return _Client.GetHomeTimeline().Result.Select(x => x.ToStatus()).ToList();
        }

        public IEnumerable<Core.Entities.Status> GetPublicTimline()
        {
            return _Client.GetPublicTimeline().Result.Select(x => x.ToStatus()).ToList();

        }
    }

    public class UserStreamingObservable : IObservable<StreamingMessage>
    {
        public AppRegistration Registration { get; private set; }

        public Mastonet.Entities.Auth Auth { get; private set; }

        public UserStreamingObservable(AppRegistration registration, Mastonet.Entities.Auth auth)
        {
            this.Registration = registration;
            this.Auth = auth;
        }

        public IDisposable Subscribe(IObserver<StreamingMessage> observer)
        {
            var streaming = new StreamingImpl(observer, this.Registration, this.Auth, client => client.GetUserStreaming());
            streaming.Start();

            return streaming;
        }
    }

    public class PublicStreamingObservable : IObservable<StreamingMessage>
    {
        public AppRegistration Registration { get; private set; }

        public Mastonet.Entities.Auth Auth { get; private set; }

        public PublicStreamingObservable(AppRegistration registration, Mastonet.Entities.Auth auth)
        {
            this.Registration = registration;
            this.Auth = auth;
        }

        public IDisposable Subscribe(IObserver<StreamingMessage> observer)
        {
            var streaming = new StreamingImpl(observer, this.Registration, this.Auth, client => client.GetPublicStreaming());
            streaming.Start();

            return streaming;
        }
    }

    internal class StreamingImpl : IDisposable
    {
        private IObserver<StreamingMessage> _Observer { get; set; }

        private MastodonClient _Client { get; set; }

        private TimelineStreaming _Streaming { get; set; }

        public StreamingImpl(IObserver<StreamingMessage> observer, AppRegistration registration, Mastonet.Entities.Auth auth, Func<MastodonClient, TimelineStreaming> factory)
        {
            _Observer = observer;
            _Client = new MastodonClient(registration, auth);
            _Streaming = factory(_Client);

            _Streaming.OnUpdate += OnUpdate;
            _Streaming.OnDelete += OnDelete;
            _Streaming.OnNotification += OnNotification;
        }

        public void Start()
        {
            _Streaming?.Start();
        }

        private void OnUpdate(object sender, StreamUpdateEventArgs e)
        {
            this._Observer?.OnNext(new UpdateStatusMessage(e.Status.ToStatus()));
        }

        private void OnDelete(object sender, StreamDeleteEventArgs e)
        {
            this._Observer?.OnNext(new DeleteStatusMessage(e.StatusId));
        }

        private void OnNotification(object sender, StreamNotificationEventArgs e)
        {
            this._Observer?.OnNext(new NotificationMessage(e.Notification.ToNotification()));
        }

        public void Dispose()
        {
            _Streaming?.Stop();
            _Streaming = null;
        }
    }
}
