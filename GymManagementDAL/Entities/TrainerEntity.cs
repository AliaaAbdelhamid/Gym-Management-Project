using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymManagementDAL.Entities
{
	public class TrainerEntity : GymUser
	{
		public Specialties Specialties { get; set; }

		public ICollection<SessionEntity> Sessions { get; set; } = null!;
	}
}
