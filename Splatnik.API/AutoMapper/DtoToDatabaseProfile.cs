using AutoMapper;
using Splatnik.API.Dtos;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.AutoMapper
{
	public class DtoToDatabaseProfile : Profile
	{
		public DtoToDatabaseProfile()
		{
			CreateMap<BudgetDto, Budget>();
			CreateMap<PeriodDto, Period>();
			CreateMap<ExpenseDto, Expense>();
			CreateMap<IncomeDto, Income>();
			CreateMap<UpdateIncomeDto, Income>();
			CreateMap<UpdateExpenseDto, Income>();
			CreateMap<DebtDto, Debt>();
			CreateMap<DebtPaymentDto, DebtPayment>();
		}
	}
}
