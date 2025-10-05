namespace GymManagementDAL.Entities
{
	public class MembershipEntity : BaseEntity
	{
		public DateTime EndDate { get; set; }

		// EF Core only maps properties that have both a get and a set (or at least a backing field).
		// Read-only, computed properties are ignored unless you explicitly configure them
		public string Status
		{
			get
			{
				if (EndDate <= DateTime.Now)
					return "Expired";
				else
					return "Active";
			}
		}
		public int MemberId { get; set; }
		public MemberEntity Member { get; set; } = null!;
		public int PlanId { get; set; }
		public PlanEntity Plan { get; set; } = null!;
	}
}
