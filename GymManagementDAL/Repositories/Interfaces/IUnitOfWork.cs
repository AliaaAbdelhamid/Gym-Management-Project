using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
		int SaveChanges();

	}
}
