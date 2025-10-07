using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
	public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
	{
		TEntity? GetById(int id);
		IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null);
		void Add(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);

	}
}
