using System;
namespace Experiments.Core.Messages.Commands
{
    public class DeleteExperiment : Command
    {
        public readonly long Id;

        public DeleteExperiment(long id, long loggedInUserId, Guid sagaId = default)
            : base(loggedInUserId, sagaId)
        {
            Id = id;
        }
    }
}
