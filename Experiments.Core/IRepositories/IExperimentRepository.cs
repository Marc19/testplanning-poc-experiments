using System;
using Domain.Common;
using Experiments.Core.Entities;
using Experiments.Core.Messages.Commands;

namespace Experiments.Core.IRepositories
{
    public interface IExperimentRepository
    {
        Result<Experiment> CreateExperiment(CreateExperiment createExperiment);

        Result DeleteExperiment(DeleteExperiment deleteExperiment);

        Result<Experiment> UpdateExperiment(UpdateExperiment updateExperiment);

        Result AddMethodsToExperiment(AddMethodsToExperiment addMethodsToExperiment);

        Result RemoveMethodsFromExperiment(RemoveMethodsFromExperiment removeMethodsFromExperiment);
    }
}
