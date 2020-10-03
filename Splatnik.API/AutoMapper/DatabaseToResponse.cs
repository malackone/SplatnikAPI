using AutoMapper;
using Splatnik.Contracts.V1.Responses;
using Splatnik.Data.Database.DbModels;

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
			CreateMap<Debt, DebtResponse>();
			CreateMap<DebtPayment, DebtPaymentResponse>();
		}
	}
}
