using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
	public class UnitOfWork : IUnitOfWork
	{
		public IMembershipRepository MembershipRepository { get; }
		public ISessionRepository SessionRepository { get; }

		public IBookingRepository BookingRepository { get; }

		private readonly Dictionary<string, object> repositories = [];
		private readonly GymDbContext _dbContext;
		public UnitOfWork(GymDbContext dbContext,
			IMembershipRepository membershipRepository,
			ISessionRepository sessionRepository,
			IBookingRepository bookingRepository)
		{
			_dbContext = dbContext;
			MembershipRepository = membershipRepository;
			SessionRepository = sessionRepository;
			BookingRepository = bookingRepository;
		}


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
