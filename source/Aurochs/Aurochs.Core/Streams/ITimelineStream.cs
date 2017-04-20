using Aurochs.Core.Entities;
using Aurochs.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Streams
{
    public interface ITimelineStream
    {
        IObservable<StreamingMessage> UserAsObservable();

        IObservable<StreamingMessage> PublicAsObservable();
    }
}
