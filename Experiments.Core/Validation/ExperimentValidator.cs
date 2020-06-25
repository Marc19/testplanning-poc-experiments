using System;
using Experiments.Core.Entities;
using FluentValidation;

namespace Experiments.Core.Validation
{
    public class ExperimentValidator : AbstractValidator<Experiment>
    {
        public ExperimentValidator()
        {
            RuleFor(exp => exp.Creator).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(exp => exp.Name).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(exp => exp.CreationDate).NotNull().NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
