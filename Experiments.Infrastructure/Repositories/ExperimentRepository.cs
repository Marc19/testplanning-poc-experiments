using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Experiments.Core;
using Experiments.Core.Entities;
using Experiments.Core.IRepositories;
using Experiments.Core.Messages.Commands;
using Experiments.Core.Validation;

namespace Experiments.Infrastructure.Repositories
{
    public class ExperimentRepository : IExperimentRepository
    {
        public static readonly List<Experiment> experimentsMemoryDatabase = new List<Experiment>();
        private static long lastIdValue = 0;

        public ExperimentRepository()
        {
        }

        public Result<Experiment> CreateExperiment(CreateExperiment createExperiment)
        {
            Experiment experiment = new Experiment(createExperiment.Creator, createExperiment.Name);

            ExperimentValidator experimentValidator = new ExperimentValidator();
            var validationResult = experimentValidator.Validate(experiment);

            if (!validationResult.IsValid)
            {
                return Result.Fail<Experiment>(string.Join(" ", validationResult.Errors));
            }

            experiment.SetId(++lastIdValue);
            experimentsMemoryDatabase.Add(experiment);

            return Result.Ok<Experiment>(experiment);
        }

        public Result DeleteExperiment(DeleteExperiment deleteExperiment)
        {
            long idToDelete = deleteExperiment.Id;

            int numberOfRemovedItems = experimentsMemoryDatabase.RemoveAll(e => e.Id == idToDelete);

            if(numberOfRemovedItems == 0)
            {
                return Result.Fail("The experiment you are trying to delete does not exist");
            }

            return Result.Ok();
        }

        public Result<Experiment> UpdateExperiment(UpdateExperiment updateExperiment)
        {
            Experiment experimentToUpdate = experimentsMemoryDatabase.Find(e => e.Id == updateExperiment.Id);

            if(experimentToUpdate == null)
            {
                return Result.Fail<Experiment>("The experiment you're trying to update does not exist");
            }

            experimentToUpdate.Creator = updateExperiment.Creator;
            experimentToUpdate.Name = updateExperiment.Name;

            return Result.Ok<Experiment>(experimentToUpdate);
        }

        public Result AddMethodsToExperiment(AddMethodsToExperiment addMethodsToExperiment)
        {
            //Will fail if the experiment does not exist
            long experimentId = addMethodsToExperiment.ExperimentId;
            List<long> methodsIds = addMethodsToExperiment.MethodsIds;

            Experiment experiment = experimentsMemoryDatabase.SingleOrDefault(e => e.Id == experimentId);

            if (experiment == null)
            {
                return Result.Fail("The provided experiment does not exist");
            }

            methodsIds.ForEach(mId => experiment.AddMethod(mId));

            return Result.Ok();
        }

        public Result RemoveMethodsFromExperiment(RemoveMethodsFromExperiment removeMethodsFromExperiment)
        {
            //Will fail if the experiment does not exist
            long experimentId = removeMethodsFromExperiment.ExperimentId;
            List<long> methodsIds = removeMethodsFromExperiment.MethodsIds;

            Experiment experiment = experimentsMemoryDatabase.SingleOrDefault(e => e.Id == experimentId);

            if (experiment == null)
            {
                return Result.Fail("The provided experiment does not exist");
            }

            methodsIds.ForEach(mId => experiment.RemoveMethod(mId));

            return Result.Ok();
        }
    }
}
