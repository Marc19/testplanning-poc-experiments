using System;
using System.Collections.Generic;

namespace Experiments.Core.Messages.Events
{
    public class MethodsAddedToExperiment : Event
    {
        public readonly long ExperimentId;

        public readonly List<long> MethodsIds;

        public MethodsAddedToExperiment(long experimentId, List<long> methodsIds,
            long loggedInUserId, Guid sagaId = default)
            : base(loggedInUserId, sagaId)
        {
            ExperimentId = experimentId;
            MethodsIds = methodsIds;
        }
    }

    public class MethodsAdditionToExperimentFailed : FailedEvent
    {
        public MethodsAdditionToExperimentFailed(string failureReason, long loggedInUserId, Guid sagaId = default)
            : base(failureReason, loggedInUserId, sagaId) { }
    }

}
