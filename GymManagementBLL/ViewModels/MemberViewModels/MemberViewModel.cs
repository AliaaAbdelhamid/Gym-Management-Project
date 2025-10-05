namespace GymManagementBLL.ViewModels.MemberViewModel
{
	public class MemberViewModel
	{
		public int Id { get; set; }
		public string? Photo { get; set; }
		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Phone { get; set; } = null!;
		public string Gender { get; set; } = null!;
		public string DateOfBirth { get; set; } = null!;
		public string Address { get; set; } = null!;
		public string? PlanName { get; set; } = null!;
		public string? MembershipStartDate { get; set; } = null!;
		public string? MembershipEndDate { get; set; } = null!;

	}
}
