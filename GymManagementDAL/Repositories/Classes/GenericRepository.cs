using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
	internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly GymDbContext _dbContext;

		public GenericRepository(GymDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public int Add(T entity)
		{
			_dbContext.Add(entity);
			return _dbContext.SaveChanges();
		}

		public int Delete(T entity)
		{
			_dbContext.Remove(entity);
			return _dbContext.SaveChanges();
		}

		public IEnumerable<T> GetAll()
			  => _dbContext.Set<T>().AsNoTracking().ToList();

		public T? GetById(int id)
		  => _dbContext.Set<T>().Find(id);

		public int Update(T entity)
		{
			_dbContext.Update(entity);
			return _dbContext.SaveChanges();
		}
	}
}
