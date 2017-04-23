using Aurochs.Core.Entities;
using Aurochs.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Streams
{
    public interface ITimelineStream
    {
        IEnumerable<Status> GetUserTimline();

        IEnumerable<Status> GetPublicTimline();

        IObservable<StreamingMessage> UserAsObservable();

        IObservable<StreamingMessage> PublicAsObservable();

        void PostPublicStatus(string text);

        void PostPrivateStatus(string text);
    }
}
