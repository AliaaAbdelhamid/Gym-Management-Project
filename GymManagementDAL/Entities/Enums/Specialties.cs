using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities.Enums
{
	[Flags]
	public enum Specialties : byte
	{
		GeneralFitness = 1,
		Yoga = 2,
		Boxing = 4,
		CrossFit = 8
	}
}
