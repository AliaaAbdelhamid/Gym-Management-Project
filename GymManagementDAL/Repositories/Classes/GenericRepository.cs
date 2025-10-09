using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
	{
		private readonly GymDbContext _dbContext;

		public GenericRepository(GymDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public void Add(TEntity entity)
		{
			_dbContext.Add(entity);
		}

		public void Delete(TEntity entity)
		{
			_dbContext.Remove(entity);
		}

		public bool Exists(Func<TEntity, bool> predicate)
		{
			return _dbContext.Set<TEntity>().Any(predicate);
		}

		public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
		{
			if (condition is not null)
				return _dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
			else
				return _dbContext.Set<TEntity>().AsNoTracking().ToList();
		}
		public TEntity? GetById(int id)
		  => _dbContext.Set<TEntity>().Find(id);

		public void Update(TEntity entity)
		{
			_dbContext.Update(entity);
		}
	}
}
