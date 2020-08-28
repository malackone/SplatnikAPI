using FluentValidation;
using Splatnik.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Validation
{
	public class CreateBudgetRequestValidator : AbstractValidator<CreateBudgetRequest>
	{
		public CreateBudgetRequestValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.NotNull()
				.MaximumLength(50);

			RuleFor(x => x.Description)
				.MaximumLength(200);

			RuleFor(x => x.UserId)
				.NotEmpty()
				.NotNull()
				.MaximumLength(450);
		}
	}
}
