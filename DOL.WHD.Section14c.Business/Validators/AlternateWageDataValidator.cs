﻿using DOL.WHD.Section14c.Domain.Models.Submission;
using FluentValidation;

namespace DOL.WHD.Section14c.Business.Validators
{
    public class AlternateWageDataValidator : BaseValidator<AlternateWageData>, IAlternateWageDataValidator
    {
        public AlternateWageDataValidator()
        {
            RuleFor(a => a.AlternateWorkDescription).NotEmpty();
            RuleFor(a => a.AlternateDataSourceUsed).NotEmpty();
            RuleFor(a => a.PrevailingWageProvidedBySource).NotNull();
            RuleFor(a => a.DataRetrieved).NotEmpty();
        }
    }
}
