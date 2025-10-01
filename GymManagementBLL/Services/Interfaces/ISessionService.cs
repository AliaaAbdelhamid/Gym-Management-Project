using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interfaces
{
	internal interface ISessionService
	{
		IEnumerable<SessionViewModel> GetAllSessions();
		SessionViewModel? GetSessionById(int sessionId);
		UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
		bool CreateSession(CreateSessionViewModel createSession);
		bool UpdateSession(int id, UpdateSessionViewModel updateSession);
		bool RemoveSession(int sessionId);
	}
}
