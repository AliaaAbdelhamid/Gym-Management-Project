using GymManagementBLL.ViewModels.MemberViewModel;

namespace GymManagementBLL.Services.Interfaces
{
	public interface IMemberService
	{
		bool CreateMember(CreateMemberViewModel CreatedMember);
		bool UpdateMemberDetails(int Id, MemberToUpdateViewModel updateViewModel);
		bool RemoveMember(int MemberId);
		MemberViewModel? GetMemberDetails(int MemberId);
		HealthRecordViewModel? GetMemberHealthRecord(int MemberId);
		MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
		IEnumerable<MemberViewModel> GetAllMembers();

	}
}
