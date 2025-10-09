using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
	public interface ISessionRepository : IGenericRepository<SessionEntity>
	{
		IEnumerable<SessionEntity> GetAllSessionsWithTrainerAndCategory();
		SessionEntity? GetSessionWithTrainerAndCategory(int SessionId);

		int GetCountOfBookedSlots(int SessionId);
	}
}
