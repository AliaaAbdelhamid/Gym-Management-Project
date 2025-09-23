using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
	internal interface IMemberRepository
	{
		MemberEntity? GetByIdAsync(int id);
		IEnumerable<MemberEntity> GetAllAsync();
		int Add(MemberEntity member);
		int Update(MemberEntity member);
		int Delete(int id);
	}
}
