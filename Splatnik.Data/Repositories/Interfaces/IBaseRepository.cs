using Splatnik.Data.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories.Interfaces
{
	public interface IBaseRepository<T> where T: BaseEntity
	{
		Task<T> CreateEntityAsync(T entity);
		Task<T> GetEntityAsync(int id);
		Task<bool> UpdateEntityAsync(T entity);
		Task<bool> DeleteEntityAsync(T entity);
	}
}
