using System;
using System.Collections.Generic;

namespace Experiments.Core.Messages.Commands
{
    public class RemoveMethodsFromExperiment : Command
    {
        public readonly long ExperimentId;

        public readonly List<long> MethodsIds;

        public RemoveMethodsFromExperiment(long experimentId, List<long> methodsIds,
            long loggedInUserId, Guid sagaId = default)
            : base(loggedInUserId, sagaId)
        {
            ExperimentId = experimentId;
            MethodsIds = methodsIds;
        }
    }
}
