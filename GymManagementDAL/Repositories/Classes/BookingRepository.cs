using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
	public class BookingRepository : GenericRepository<BookingEntity>, IBookingRepository
	{
		private readonly GymDbContext _dbContext;

		public BookingRepository(GymDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public IEnumerable<BookingEntity> GetBySessionId(int sessionId)
		{
			return _dbContext.Bookings.Include(X => X.Member)
									  .Where(X => X.SessionId == sessionId).ToList();
		}

	}
}
