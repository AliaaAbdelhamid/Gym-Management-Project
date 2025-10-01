using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
	internal class MemberRepository : IMemberRepository
	{
		private readonly GymDbContext dbContext;

		public MemberRepository(GymDbContext dbContext)
		{
			dbContext = dbContext;
		}
		public MemberEntity? GetById(int id) => dbContext.Members.Find(id);
		public IEnumerable<MemberEntity> GetAll() => dbContext.Members.ToList();
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
			var Member = GetById(id);
			if (Member == null)
				return 0;
			dbContext.Members.Remove(Member);
			return dbContext.SaveChanges();
		}
	}
}
