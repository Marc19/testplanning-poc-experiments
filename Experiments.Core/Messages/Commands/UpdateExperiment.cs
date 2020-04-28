using System;
namespace Experiments.Core.Messages.Commands
{
    public class UpdateExperiment : Command
    {
        public readonly long Id;

        public readonly string Creator;

        public readonly string Name;

        public UpdateExperiment(long id, string creator, string name, long loggedInUserId, Guid sagaId = default)
            : base(loggedInUserId, sagaId)
        {
            Id = id;
            Creator = creator;
            Name = name;
        }
    }
}
