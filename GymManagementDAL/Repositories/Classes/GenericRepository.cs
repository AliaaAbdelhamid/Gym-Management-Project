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
	internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
	{
		private readonly GymDbContext _dbContext;

		public GenericRepository(GymDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public int Add(TEntity entity)
		{
			_dbContext.Add(entity);
			return _dbContext.SaveChanges();
		}

		public int Delete(TEntity entity)
		{
			_dbContext.Remove(entity);
			return _dbContext.SaveChanges();
		}

		public IEnumerable<TEntity> GetAll()
			  => _dbContext.Set<TEntity>().AsNoTracking().ToList();

		public TEntity? GetById(int id)
		  => _dbContext.Set<TEntity>().Find(id);

		public int Update(TEntity entity)
		{
			_dbContext.Update(entity);
			return _dbContext.SaveChanges();
		}
	}
}
