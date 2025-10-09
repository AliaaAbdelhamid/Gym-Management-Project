using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class AnalyticsService : IAnalyticsService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AnalyticsService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public AnalyticsViewModel GetAnalyticsData()
		{
			var Sessions = _unitOfWork.GetRepository<SessionEntity>().GetAll();
			return new AnalyticsViewModel()
			{
				ActiveMembers = _unitOfWork.GetRepository<MembershipEntity>().GetAll(X => X.Status == "Active").Count(),
				TotalMembers = _unitOfWork.GetRepository<MemberEntity>().GetAll().Count(),
				TotalTrainers = _unitOfWork.GetRepository<TrainerEntity>().GetAll().Count(),
				UpcomingSessions = Sessions.Where(X => X.StartDate > DateTime.Now).Count(),
				OngoingSessions = Sessions.Where(X => X.StartDate <= DateTime.Now && X.EndDate >= DateTime.Now).Count(),
				CompletedSessions = Sessions.Where(X => X.EndDate < DateTime.Now).Count(),
			};
		}
	}
}
