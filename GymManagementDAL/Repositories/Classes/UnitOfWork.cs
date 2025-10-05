using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
	public class UnitOfWork : IUnitOfWork
	{
		public UnitOfWork(GymDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		private readonly Dictionary<string, object> repositories = [];
		private readonly GymDbContext _dbContext;

		public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
		{
			var typeName = typeof(TEntity).Name;
			if (repositories.TryGetValue(typeName, out object? value))
				return (IGenericRepository<TEntity>)value;
			var Repo = new GenericRepository<TEntity>(_dbContext);
			repositories[typeName] = Repo;
			return Repo;
		}

		public int SaveChanges()
		=> _dbContext.SaveChanges();
	}
}
