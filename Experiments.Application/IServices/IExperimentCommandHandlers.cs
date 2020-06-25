using System;
using Experiments.Core.Messages.Commands;

namespace Experiments.Application.IServices
{
    public interface IExperimentCommandHandlers
    {
        void Handle(Command command);
    }
}
