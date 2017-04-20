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

namespace Aurochs.Linkage.Streams
{
    public class TimelineStream : ITimelineStream
    {
        private AppRegistration _AppRegistration { get; set; }

        private MastodonClient _Client { get; set; }

        private TimelineStreaming _Steaming { get; set; }

        public TimelineStream(AppRegistration appRegistration)
        {
            this._AppRegistration = appRegistration;

            _Client = new MastodonClient(this._AppRegistration);
            _Steaming = _Client.GetUserStreaming();
        }

        public IObservable<StreamingMessage> UserAsObservable(ApplicationRegistration registration)
        {
            var appRegistration = registration.ToMastonetAppRegistration();
            return new StreamingObservable(appRegistration);
        }
    }

    public class StreamingObservable : IObservable<StreamingMessage>
    {
        public AppRegistration Registration { get; private set; }

        public StreamingObservable(AppRegistration registration)
        {
            this.Registration = registration;
        }
        
        public IDisposable Subscribe(IObserver<StreamingMessage> observer)
        {
            var streaming = new StreamingImpl(this.Registration);
            streaming.Start();

            return streaming;
        }
    }

    public class StreamingImpl : IDisposable
    {
        private IObserver<StreamingMessage> _Observer { get; set; }

        private MastodonClient _Client { get; set; }

        private TimelineStreaming _Streaming { get; set;}

        public StreamingImpl(AppRegistration registration)
        {
            _Client = new MastodonClient(registration);
            _Streaming = _Client.GetUserStreaming();

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
