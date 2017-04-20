using Aurochs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Messages
{
    public enum MessageType
    {
        Unknown,
        UpdateStatus,
        DeleteStatus,
        Notification,
    }

    public abstract class StreamingMessage
    {
        public MessageType Type { get { return this.GetMessageType(); } }
        
        protected abstract MessageType GetMessageType();
    }

    public class UpdateStatusMessage : StreamingMessage
    {
        public Status Status { get; }

        public UpdateStatusMessage(Status status)
        {
            this.Status = status;
        }

        protected override MessageType GetMessageType()
        {
            return MessageType.UpdateStatus;
        }
    }

    public class DeleteStatusMessage : StreamingMessage
    {
        public long StatusId { get; }

        public DeleteStatusMessage(long statusId)
        {
            this.StatusId = statusId;
        }

        protected override MessageType GetMessageType()
        {
            return MessageType.DeleteStatus;
        }
    }

    public class NotificationMessage : StreamingMessage
    {
        public Notification Notification { get; }

        public NotificationMessage(Notification notification)
        {
            this.Notification = notification;
        }

        protected override MessageType GetMessageType()
        {
            return MessageType.Notification;
        }
    }
}
