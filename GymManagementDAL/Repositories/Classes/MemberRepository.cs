using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
	internal class MemberRepository : IMemberRepository
	{
		private readonly GymDbContext dbContext = new GymDbContext();
		public MemberEntity? GetByIdAsync(int id) => dbContext.Members.Find(id);
		public IEnumerable<MemberEntity> GetAllAsync() => dbContext.Members.ToList();
		public int Add(MemberEntity member)
		{
			dbContext.Members.Add(member);
			return dbContext.SaveChanges();
		}
		public int Update(MemberEntity member)
		{
			dbContext.Members.Update(member);
			return dbContext.SaveChanges();
		}
		public int Delete(int id)
		{
			var Member = GetByIdAsync(id);
			if (Member == null)
				return 0;
			dbContext.Members.Remove(Member);
			return dbContext.SaveChanges();
		}
	}
}
