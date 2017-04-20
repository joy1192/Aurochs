using Aurochs.Core.Entities;
using Infrastructure.Flux.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ActionMessages
{
    public class TimelineInitializeMessage : ActionMessage
    {
        public List<Status> TweetContent { get; }
    }

    public class UserTimelineInitializeMessage : TimelineInitializeMessage
    {

    }

    public class PublicTimelineInitializeMessage : TimelineInitializeMessage
    {
    }
}
