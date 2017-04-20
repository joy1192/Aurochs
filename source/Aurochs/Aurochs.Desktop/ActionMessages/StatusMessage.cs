using Aurochs.Core.Entities;
using Infrastructure.Flux.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ActionMessages
{
    public enum StatusSource
    {
        User,
        Public
    }

    public class UpdateStatusActionMessage : ActionMessage
    {
        public Status Status { get; }

        public StatusSource Source { get; }

        public UpdateStatusActionMessage(Status status, StatusSource source)
        {
            this.Status = status;
            this.Source = source;
        }
    }

    public class DeleteStatusActionMessage : ActionMessage
    {
        public long StatusId { get; }

        public StatusSource Source { get; }

        public DeleteStatusActionMessage(long statusId, StatusSource source)
        {
            this.StatusId = statusId;
            this.Source = source;
        }
    }
}
