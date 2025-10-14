using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class SessionService : ISessionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public IEnumerable<SessionViewModel> GetAllSessions()
		{
			var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory().OrderByDescending(X => X.StartDate);

			if (sessions == null || !sessions.Any()) return Enumerable.Empty<SessionViewModel>();

			var MappedSessions = _mapper.Map<IEnumerable<SessionEntity>, IEnumerable<SessionViewModel>>(sessions);

			foreach (var session in MappedSessions)
			{
				session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
			}
			return MappedSessions;

		}
		public SessionViewModel? GetSessionById(int sessionId)
		{
			var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);

			if (session == null)
				return null;

			var MappedSession = _mapper.Map<SessionEntity, SessionViewModel>(session);
			MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
			return MappedSession;
		}
		public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
		{
			var session = _unitOfWork.GetRepository<SessionEntity>().GetById(sessionId);

			if (!IsSessionValidForUpdating(session!)) return null;

			return _mapper.Map<UpdateSessionViewModel>(session);
		}
		public bool CreateSession(CreateSessionViewModel createSession)
		{
			try
			{
				var repo = _unitOfWork.GetRepository<SessionEntity>();

				if (!IsTrainerExists(createSession.TrainerId)) return false;
				if (!IsCategoryExists(createSession.CategoryId)) return false;
				if (!IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;
				var sessionEntity = _mapper.Map<SessionEntity>(createSession);
				sessionEntity.CreatedAt = DateTime.Now;
				repo.Add(sessionEntity);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public bool UpdateSession(int id, UpdateSessionViewModel updateSession)
		{
			try
			{
				var repo = _unitOfWork.GetRepository<SessionEntity>();
				var session = repo.GetById(id);

				if (!IsSessionValidForUpdating(session!)) return false;
				if (!IsTrainerExists(updateSession.TrainerId)) return false;
				if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;

				_mapper.Map(updateSession, session);
				session!.UpdatedAt = DateTime.Now;

				repo.Update(session);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public bool RemoveSession(int sessionId)
		{
			try
			{
				var repo = _unitOfWork.GetRepository<SessionEntity>();
				var session = repo.GetById(sessionId);

				if (!IsSessionValidForRemoving(session!)) return false;

				repo.Delete(session!);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown()
		{
			var trainers = _unitOfWork.GetRepository<TrainerEntity>().GetAll();
			return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
		}
		public IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown()
		{

			var categories = _unitOfWork.GetRepository<CategoryEntity>().GetAll();
			return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
		}

		#region Helper Methods
		private bool IsSessionValidForUpdating(SessionEntity session)
		{
			// Only future sessions with no bookings
			return session.StartDate > DateTime.Now &&
		   _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) == 0;
		}
		private bool IsSessionValidForRemoving(SessionEntity session)
		{
			//  Only completed sessions with no bookings
			return session.EndDate < DateTime.Now &&
				   _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) == 0;
		}
		private bool IsTrainerExists(int id)
		{
			var trainer = _unitOfWork.GetRepository<TrainerEntity>().GetById(id);
			return trainer is null ? false : true;
		}
		private bool IsCategoryExists(int id)
		{
			var category = _unitOfWork.GetRepository<CategoryEntity>().GetById(id);
			return category is null ? false : true;
		}
		private bool IsValidDateRange(DateTime StartDate, DateTime EndDate)
		{
			return EndDate > StartDate && StartDate > DateTime.Now;
		}

		#endregion
	}
}