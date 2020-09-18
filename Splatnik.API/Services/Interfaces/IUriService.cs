using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface IUriService
	{
		Uri GetBudgetUri(string budgetId);
		Uri GetPeriodUri(string periodId);
		Uri GetConfirmationLink(string email, string token);
	}
}
