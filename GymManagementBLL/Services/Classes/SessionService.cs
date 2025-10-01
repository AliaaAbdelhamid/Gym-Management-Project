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
			var sessions = _unitOfWork.GetRepository<SessionEntity>().GetAll();

			if (sessions == null || !sessions.Any())
				return Enumerable.Empty<SessionViewModel>();

			var trainers = _unitOfWork.GetRepository<TrainerEntity>().GetAll().ToList();
			var categories = _unitOfWork.GetRepository<CategoryEntity>().GetAll().ToList();

			return sessions.Select(s =>
			{
				var viewModel = _mapper.Map<SessionViewModel>(s);

				var trainer = trainers.FirstOrDefault(t => t.Id == s.TrainerId);
				var category = categories.FirstOrDefault(c => c.Id == s.CategoryId);

				viewModel.TrainerName = trainer?.Name ?? "Unknown";
				viewModel.CategoryName = category?.CategoryName ?? "Unknown";
				viewModel.AvailableSlots = s.Capacity;

				return viewModel;
			});
		}

		public SessionViewModel? GetSessionById(int sessionId)
		{
			var session = _unitOfWork.GetRepository<SessionEntity>().GetById(sessionId);

			if (session == null)
				return null;

			var trainer = _unitOfWork.GetRepository<TrainerEntity>().GetById(session.TrainerId);
			var category = _unitOfWork.GetRepository<CategoryEntity>().GetById(session.CategoryId);

			var viewModel = _mapper.Map<SessionViewModel>(session);
			viewModel.TrainerName = trainer?.Name ?? "Unknown";
			viewModel.CategoryName = category?.CategoryName ?? "Unknown";
			viewModel.AvailableSlots = session.Capacity;

			return viewModel;
		}

		public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
		{
			var session = _unitOfWork.GetRepository<SessionEntity>().GetById(sessionId);

			if (session == null)
				return null;
			return _mapper.Map<UpdateSessionViewModel>(session);
		}

		public bool CreateSession(CreateSessionViewModel createSession)
		{
			try
			{
				var repo = _unitOfWork.GetRepository<SessionEntity>();

				if (!IsTrainerExists(createSession.TrainerId)) return false;
				if (!IsCategoryExists(createSession.TrainerId)) return false;
				if (!CheckSessionDateRange(createSession.StartDate, createSession.EndDate)) return false;
				var sessionEntity = _mapper.Map<SessionEntity>(createSession);

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
			if (updateSession == null)
				return false;

			try
			{
				var repo = _unitOfWork.GetRepository<SessionEntity>();
				var session = repo.GetById(id);

				if (session == null)
					return false;

				if (!IsTrainerExists(updateSession.TrainerId)) return false;
				if (!CheckSessionDateRange(updateSession.StartDate, updateSession.EndDate)) return false;

				_mapper.Map(updateSession, session);
				session.UpdatedAt = DateTime.Now;

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

				if (session is null || session.EndDate > DateTime.Now) return false;

				repo.Delete(session);
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

		private bool CheckSessionDateRange(DateTime StartDate, DateTime EndDate)
		{
			return EndDate <= StartDate ? false : true;
		}

		#endregion
	}
}