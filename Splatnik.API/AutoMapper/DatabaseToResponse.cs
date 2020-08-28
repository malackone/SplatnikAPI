using AutoMapper;
using Splatnik.Contracts.V1.Responses;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.AutoMapper
{
	public class DatabaseToResponse : Profile
	{
		public DatabaseToResponse()
		{
			CreateMap<Budget, BudgetResponse>();
			CreateMap<Period, PeriodResponse>();
			CreateMap<Expense, ExpenseResponse>();
			CreateMap<Income, IncomeResponse>();
		}
	}
}
