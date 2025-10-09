using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		public IMembershipRepository MembershipRepository { get; set; }
		public ISessionRepository SessionRepository { get; set; }
		IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
		int SaveChanges();

	}
}
