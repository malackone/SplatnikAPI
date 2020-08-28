using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface IBudgetService
	{
		Task<Budget> CreateBudgetAsync(CreateBudgetRequest budgetRequest);
		Task<Budget> GetBudgetAsync(int budgetId);
	}
}
