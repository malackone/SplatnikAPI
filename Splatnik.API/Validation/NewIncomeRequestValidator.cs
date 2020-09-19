using FluentValidation;
using Splatnik.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Validation
{
    public class NewIncomeRequestValidator : AbstractValidator<NewIncomeRequest>
    {
        public NewIncomeRequestValidator()
        {
            RuleFor(x => x.IncomeDate)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(200);

            RuleFor(x => x.IncomeValue)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .ScalePrecision(2, 10);
        }
    }
}
