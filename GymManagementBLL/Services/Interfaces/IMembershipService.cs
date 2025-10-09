using GymManagementBLL.ViewModels.MembershipViewModels;

namespace GymManagementBLL.Services.Interfaces
{
	public interface IMembershipService
	{
		IEnumerable<MemberShipViewModel> GetAllMemberShips();
		IEnumerable<MemberShipForMemberViewModel> GetMembershipsForSpecificMember(int MemberId);
		bool CreateMembership(CreateMemberShipViewModel CreatedMemberShip);
		bool DeleteMemberShip(int MemberId);
		bool RenewMemberShip(int MemberId, int PlanId);

		IEnumerable<PlanSelectListViewModel> GetPlansForDropDown();
		IEnumerable<MemberSelectListViewModel> GetMembersForDropDown();

	}
}
