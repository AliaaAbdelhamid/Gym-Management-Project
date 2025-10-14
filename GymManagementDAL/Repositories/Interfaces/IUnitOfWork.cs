using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		public IMembershipRepository MembershipRepository { get; }
		public ISessionRepository SessionRepository { get; }
		public IBookingRepository BookingRepository { get; }
		IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
		int SaveChanges();

	}
}
