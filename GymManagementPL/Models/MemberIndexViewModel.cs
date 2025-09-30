using GymManagementBLL.ViewModels.MemberViewModel;

namespace GymManagementPL.Models
{
    public class MemberIndexViewModel
    {
        public IEnumerable<MemberViewModel> Members { get; set; } = new List<MemberViewModel>();
        public CreateMemberViewModel CreateMember { get; set; } = new CreateMemberViewModel();
        public UpdateMemberViewModel UpdateMemberDetails { get; set; } = new UpdateMemberViewModel();
        public HealthRecordViewModel UpdateMemberHealthRecord { get; set; } = new HealthRecordViewModel();
    }
}