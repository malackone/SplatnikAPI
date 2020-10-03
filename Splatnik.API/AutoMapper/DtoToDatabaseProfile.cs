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
			CreateMap<UpdateExpenseDto, Income>();
			
			CreateMap<IncomeDto, Income>();
			CreateMap<UpdateIncomeDto, Income>();
			
			CreateMap<DebtDto, Debt>();
			CreateMap<UpdateDebtDto, Debt>();
			
			CreateMap<DebtPaymentDto, DebtPayment>();
			CreateMap<UpdateDebtPaymentDto, DebtPayment>();

			CreateMap<CreditDto, Credit>();
			CreateMap<UpdateCreditDto, Credit>();

			CreateMap<CreditPaymentDto, CreditPayment>();
			CreateMap<UpdateCreditPaymentDto, CreditPayment>();
		}
	}
}
