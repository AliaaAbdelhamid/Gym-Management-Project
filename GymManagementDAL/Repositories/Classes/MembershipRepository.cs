using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
	public class MembershipRepository : GenericRepository<MembershipEntity>, IMembershipRepository
	{
		private readonly GymDbContext _dbContext;

		public MembershipRepository(GymDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public IEnumerable<MembershipEntity> GetAllMembershipsWithMemberAndPlan(Func<MembershipEntity, bool> predicate)
		{
			return _dbContext.Memberships.Include(X => X.Plan).Include(X => X.Member).Where(predicate).ToList();
		}
	}
}
