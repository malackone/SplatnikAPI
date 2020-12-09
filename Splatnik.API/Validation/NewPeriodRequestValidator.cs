using FluentValidation;
using FluentValidation.Validators;
using Splatnik.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Validation
{
	public class NewPeriodRequestValidator : AbstractValidator<PeriodRequest>
	{
		public NewPeriodRequestValidator()
		{
			RuleFor(x => x.FirstDay)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.LastDay)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.DisplayName)
				.NotNull()
				.NotEmpty()
				.MaximumLength(7);

			RuleFor(x => x.Notes)
				.MaximumLength(1000);
		}
	}
}
