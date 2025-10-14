using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
	public class SessionRepository : GenericRepository<SessionEntity>, ISessionRepository
	{
		private readonly GymDbContext _dbContext;

		public SessionRepository(GymDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public IEnumerable<SessionEntity> GetAllSessionsWithTrainerAndCategory(Func<SessionEntity, bool>? condition = null)
		{
			if (condition is null)
				return _dbContext.Sessions.Include(X => X.Trainer)
					.Include(X => X.Category)
					.ToList();
			else
				return _dbContext.Sessions.Include(X => X.Trainer)
					.Include(X => X.Category)
					.Where(condition).ToList();
		}

		public int GetCountOfBookedSlots(int SessionId)
		{
			return _dbContext.Bookings.Where(X => X.SessionId == SessionId).Count();
		}

		public SessionEntity? GetSessionWithTrainerAndCategory(int SessionId)
		{
			return _dbContext.Sessions.Include(X => X.Trainer)
									  .Include(X => X.Category).FirstOrDefault(X => X.Id == SessionId);
		}
	}
}
