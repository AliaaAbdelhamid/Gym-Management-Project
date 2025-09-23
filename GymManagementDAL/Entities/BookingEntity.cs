using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	public class BookingEntity : BaseEntity
	{
		public bool IsAttended { get; set; }
		public int MemberId { get; set; }
		public MemberEntity Member { get; set; } = null!;
		public int SessionId { get; set; }
		public SessionEntity Session { get; set; } = null!;
	}
}
