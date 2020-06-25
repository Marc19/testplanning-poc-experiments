using System;
using System.Collections.Generic;

namespace Experiments.Core.Messages.Events
{
    public class MethodsRemovedFromExperiment : Event
    {
        public readonly long ExperimentId;

        public readonly List<long> MethodsIds;

        public MethodsRemovedFromExperiment(long experimentId, List<long> methodsIds,
            long loggedInUserId, Guid sagaId = default)
            : base(loggedInUserId, sagaId)
        {
            ExperimentId = experimentId;
            MethodsIds = methodsIds;
        }
    }

    public class MethodsRemovalFromExperimentFailed : FailedEvent
    {
        public MethodsRemovalFromExperimentFailed(string failureReason, long loggedInUserId, Guid sagaId = default)
            : base(failureReason, loggedInUserId, sagaId) { }
    }
}
