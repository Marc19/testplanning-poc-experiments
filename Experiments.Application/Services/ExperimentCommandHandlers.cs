using System;
using System.Linq;
using Domain.Common;
using Experiments.Application.IServices;
using Experiments.Core;
using Experiments.Core.Entities;
using Experiments.Core.IKafka;
using Experiments.Core.IRepositories;
using Experiments.Core.Messages.Commands;
using Experiments.Core.Messages.Events;
using Microsoft.Extensions.Configuration;

namespace Experiments.Application.Services
{
    public class ExperimentCommandHandlers : IExperimentCommandHandlers
    {
        private readonly IExperimentRepository _repository;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IConfiguration Configuration;
        private readonly string EXPERIMENT_TOPIC;

        public ExperimentCommandHandlers(IExperimentRepository repository, IKafkaProducer kafkaProducer, IConfiguration configuration)
        {
            _repository = repository;
            _kafkaProducer = kafkaProducer;
            Configuration = configuration;
            EXPERIMENT_TOPIC = Configuration["ExperimentsTopic"];
        }

        public void Handle(Command command)
        {
            switch (command)
            {
                case CreateExperiment c: HandleCreateExperiment(c); break;
                case DeleteExperiment d: HandleDeleteExperiment(d); break;
                case UpdateExperiment u: HandleUpdateExperiment(u); break;
                case AddMethodsToExperiment a: HandleAddMethodsToExperiment(a); break;
                case RemoveMethodsFromExperiment r: HandleRemoveMethodsFromExperiment(r); break;
            }
        }

        private void HandleCreateExperiment(CreateExperiment createExperiment)
        {
            Result<Experiment> experimentCreationResult = _repository.CreateExperiment(createExperiment);

            if (experimentCreationResult.IsFailure)
            {
                ExperimentCreationFailed failedExperiment =
                    new ExperimentCreationFailed(
                        experimentCreationResult.Error,
                        createExperiment.LoggedInUserId,
                        createExperiment.SagaId
                    );
                _kafkaProducer.Produce(failedExperiment, EXPERIMENT_TOPIC);
                return;
            }

            ExperimentCreated createdExperiment = new ExperimentCreated(
                experimentCreationResult.Value.Id,
                experimentCreationResult.Value.Creator,
                experimentCreationResult.Value.Name,
                experimentCreationResult.Value.CreationDate,
                createExperiment.LoggedInUserId,
                createExperiment.SagaId
            );

            _kafkaProducer.Produce(createdExperiment, EXPERIMENT_TOPIC);

        }

        private void HandleDeleteExperiment(DeleteExperiment deleteExperiment)
        {
            Result deletionResult = _repository.DeleteExperiment(deleteExperiment);

            if (deletionResult.IsFailure)
            {
                ExperimentDeletionFailed failedExperimentDeletion =
                    new ExperimentDeletionFailed(
                        deletionResult.Error,
                        deleteExperiment.LoggedInUserId,
                        deleteExperiment.SagaId
                    );
                _kafkaProducer.Produce(failedExperimentDeletion, EXPERIMENT_TOPIC);
                return;
            }

            ExperimentDeleted deletedExperiment =
                new ExperimentDeleted(
                    deleteExperiment.Id,
                    deleteExperiment.LoggedInUserId,
                    deleteExperiment.SagaId
                );
            _kafkaProducer.Produce(deletedExperiment, EXPERIMENT_TOPIC);
        }

        private void HandleUpdateExperiment(UpdateExperiment updateExperiment)
        {
            Result<Experiment> updatedExperimentResult = _repository.UpdateExperiment(updateExperiment);

            if (updatedExperimentResult.IsFailure)
            {
                ExperimentUpdateFailed failedExperimentUpdate =
                    new ExperimentUpdateFailed(
                        updatedExperimentResult.Error,
                        updateExperiment.LoggedInUserId,
                        updateExperiment.SagaId
                    );
                _kafkaProducer.Produce(failedExperimentUpdate, EXPERIMENT_TOPIC);
                return;
            }

            ExperimentUpdated updatedExperiment = new ExperimentUpdated(
                updatedExperimentResult.Value.Id,
                updatedExperimentResult.Value.Creator,
                updatedExperimentResult.Value.Name,
                updatedExperimentResult.Value.CreationDate,
                updateExperiment.LoggedInUserId,
                updateExperiment.SagaId
            );

            _kafkaProducer.Produce(updatedExperiment, EXPERIMENT_TOPIC);
        }

        private void HandleAddMethodsToExperiment(AddMethodsToExperiment addMethodsToExperiment)
        {
            Result additionResult = _repository.AddMethodsToExperiment(addMethodsToExperiment);

            if (additionResult.IsFailure)
            {
                MethodsAdditionToExperimentFailed failedMethodsAdditionToExperiment =
                    new MethodsAdditionToExperimentFailed(
                        additionResult.Error,
                        addMethodsToExperiment.LoggedInUserId,
                        addMethodsToExperiment.SagaId
                    );
                _kafkaProducer.Produce(failedMethodsAdditionToExperiment, EXPERIMENT_TOPIC);
                return;
            }

            MethodsAddedToExperiment methodsAddedToExperiment = new MethodsAddedToExperiment(
                addMethodsToExperiment.ExperimentId,
                addMethodsToExperiment.MethodsIds.Select(mId => mId).ToList(),
                addMethodsToExperiment.LoggedInUserId,
                addMethodsToExperiment.SagaId
            );

            _kafkaProducer.Produce(methodsAddedToExperiment, EXPERIMENT_TOPIC);
        }

        private void HandleRemoveMethodsFromExperiment(RemoveMethodsFromExperiment removeMethodsFromExperiment)
        {
            Result removalResult = _repository.RemoveMethodsFromExperiment(removeMethodsFromExperiment);

            if (removalResult.IsFailure)
            {
                MethodsRemovalFromExperimentFailed failedMethodsRemovalFromExperiment =
                    new MethodsRemovalFromExperimentFailed(
                        removalResult.Error,
                        removeMethodsFromExperiment.LoggedInUserId,
                        removeMethodsFromExperiment.SagaId
                    );
                _kafkaProducer.Produce(failedMethodsRemovalFromExperiment, EXPERIMENT_TOPIC);
                return;
            }

            MethodsRemovedFromExperiment methodsRemovedFromExperiment = new MethodsRemovedFromExperiment(
                removeMethodsFromExperiment.ExperimentId,
                removeMethodsFromExperiment.MethodsIds.Select(mId => mId).ToList(),
                removeMethodsFromExperiment.LoggedInUserId,
                removeMethodsFromExperiment.SagaId
            );

            _kafkaProducer.Produce(methodsRemovedFromExperiment, EXPERIMENT_TOPIC);
        }

    }
}
