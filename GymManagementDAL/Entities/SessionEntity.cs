using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	public class SessionEntity : BaseEntity
	{

		public string Description { get; set; } = null!;
		public int Capacity { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public ICollection<BookingEntity> SessionMembers { get; set; } = null!;
		public int TrainerId { get; set; }
		public TrainerEntity Trainer { get; set; } = null!;

		public int CategoryId { get; set; }
		public CategoryEntity Category { get; set; } = null!;
	}
}
