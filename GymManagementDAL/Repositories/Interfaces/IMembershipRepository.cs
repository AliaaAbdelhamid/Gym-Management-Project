using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
	public interface IMembershipRepository : IGenericRepository<MembershipEntity>
	{
		IEnumerable<MembershipEntity> GetAllMembershipsWithMemberAndPlan(Func<MembershipEntity, bool> predicate);
	}
}
