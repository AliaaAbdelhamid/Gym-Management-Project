using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	internal class PlanEntity : BaseEntity
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public int DurationDays { get; set; }
		public decimal Price { get; set; }
		public bool IsActive { get; set; }

		public ICollection<MembershipEntity> PlanMembers { get; set; } = null!;

	}
}
