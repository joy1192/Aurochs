﻿using Aurochs.Core.Streams;
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

        private string _AccessToken { get; set; }

        public TimelineStream(ApplicationRegistration appRegistration, string accessToken)
        {
            this._AppRegistration = appRegistration.ToMastonetAppRegistration();
            this._AccessToken = accessToken;

            _Client = new MastodonClient(this._AppRegistration, accessToken);
            _Steaming = _Client.GetUserStreaming();
        }

        public IObservable<StreamingMessage> UserAsObservable()
        {
            return new UserStreamingObservable(this._AppRegistration, this._AccessToken);
        }

        public IObservable<StreamingMessage> PublicAsObservable()
        {
            return new PublicStreamingObservable(_AppRegistration, this._AccessToken);
        }
    }

    public class UserStreamingObservable : IObservable<StreamingMessage>
    {
        public AppRegistration Registration { get; private set; }

        public string AccessToken { get; private set; }

        public UserStreamingObservable(AppRegistration registration, string accessToken)
        {
            this.Registration = registration;
            this.AccessToken = accessToken;
        }

        public IDisposable Subscribe(IObserver<StreamingMessage> observer)
        {
            var streaming = new StreamingImpl(observer, this.Registration, this.AccessToken, client => client.GetUserStreaming());
            streaming.Start();

            return streaming;
        }
    }

    public class PublicStreamingObservable : IObservable<StreamingMessage>
    {
        public AppRegistration Registration { get; private set; }

        public string AccessToken { get; private set; }

        public PublicStreamingObservable(AppRegistration registration, string accessToken)
        {
            this.Registration = registration;
            this.AccessToken = accessToken;
        }

        public IDisposable Subscribe(IObserver<StreamingMessage> observer)
        {
            var streaming = new StreamingImpl(observer, this.Registration, this.AccessToken, client => client.GetPublicStreaming());
            streaming.Start();

            return streaming;
        }
    }

    internal class StreamingImpl : IDisposable
    {
        private IObserver<StreamingMessage> _Observer { get; set; }

        private MastodonClient _Client { get; set; }

        private TimelineStreaming _Streaming { get; set; }

        public StreamingImpl(IObserver<StreamingMessage> observer, AppRegistration registration, string accessToken, Func<MastodonClient, TimelineStreaming> factory)
        {
            _Observer = observer;
            _Client = new MastodonClient(registration, accessToken);
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
