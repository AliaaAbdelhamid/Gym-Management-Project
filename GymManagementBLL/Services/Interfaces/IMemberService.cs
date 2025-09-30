using GymManagementBLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
	public interface IMemberService
	{
		bool CreateMember(CreateMemberViewModel CreatedMember);
		bool UpdateMemberDetails(int Id,MemberToUpdateViewModel updateViewModel);
		bool UpdateMemberHealthRecord(int Id, HealthRecordViewModel UpdatedHealthRecord);
		bool RemoveMember(int MemberId);
		MemberViewModel? GetMemberDetails(int MemberId);
		HealthRecordViewModel? GetMemberHealthRecord(int MemberId);
		MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
		IEnumerable<MemberViewModel> GetAllMembers();

	}
}
