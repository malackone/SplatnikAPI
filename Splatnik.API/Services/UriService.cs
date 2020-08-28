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
			return new Uri(_baseUri + ApiRoutes.Budget.Get.Replace("{budgetId}", budgetId));
		}
	}
}
