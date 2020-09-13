using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
	public class UriService : IUriService
	{
		private readonly string _baseUri;

		public UriService(string baseUri)
		{
			_baseUri = baseUri;
		}

		public Uri GetBudgetUri(string budgetId)
		{
			return new Uri(_baseUri + ApiRoutes.User.UserBudget.Replace("{budgetId}", budgetId));
		}

		public Uri GetConfirmationLink(string email, string token)
		{
			return new Uri(_baseUri + ApiRoutes.Identity.ConfirmEmail + "/" + email + "/" + token);
		}
	}
}
