using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	internal class CategoryEntity:BaseEntity
	{
		public Categories CategoryName { get; set; }

		public ICollection<SessionEntity> Sessions { get; set; } = null!;

	}
}
