using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	internal class MemberEntity : GymUser
	{
		public string? Photo { get; set; }
		public HealthRecordEntity HealthRecord { get; set; } = null!;
		public ICollection<BookingEntity> MemberSessions { get; set; } = null!;

		public ICollection<MembershipEntity> MemberPlans { get; set; } = null!;
	}
}
