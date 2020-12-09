using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Splatnik.Data.Database;
using Splatnik.Data.Domain;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
	{
		private readonly DataContext dataContext;
		private DbSet<T> entities;

		public BaseRepository(DataContext dataContext)
		{
			this.dataContext = dataContext;
			entities = this.dataContext.Set<T>();

		}

		public async Task<T> CreateEntityAsync(T entity)
		{
			entities.Add(entity);
			await dataContext.SaveChangesAsync();
			return entity;
		}

		public async Task<T> GetEntityAsync(int id)
		{
			return await entities.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<bool> DeleteEntityAsync(T entity)
		{
			entities.Remove(entity);
			return (await dataContext.SaveChangesAsync()) > 0;
		}

		public async Task<bool> UpdateEntityAsync(T entity)
		{
			entities.Update(entity);
			return (await dataContext.SaveChangesAsync() > 0);
		}
	}
}
