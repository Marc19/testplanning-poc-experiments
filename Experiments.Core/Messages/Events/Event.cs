using System;

namespace Experiments.Core.Messages.Events
{
    public abstract class Event : Message
    {
        public Event(long loggedInUserId, Guid sagaId) : base(loggedInUserId, sagaId) { }
    }
}
