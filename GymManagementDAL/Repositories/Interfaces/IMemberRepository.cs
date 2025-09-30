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
		MemberEntity? GetById(int id);
		IEnumerable<MemberEntity> GetAll();
		int Add(MemberEntity member);
		int Update(MemberEntity member);
		int Delete(int id);
	}
}
