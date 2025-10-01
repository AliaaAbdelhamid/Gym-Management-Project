using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class TrainerService : ITrainerService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public bool CreateTrainer(CreateTrainerViewModel createTrainer)
		{
			try
			{
				var Repo = _unitOfWork.GetRepository<TrainerEntity>();

				if (IsEmailUnique(createTrainer.Email) || IsPhoneUnique(createTrainer.Phone)) return false;
				var TrainerEntity = _mapper.Map<CreateTrainerViewModel, TrainerEntity>(createTrainer);


				Repo.Add(TrainerEntity);

				return _unitOfWork.SaveChanges() > 0;


			}
			catch (Exception)
			{
				return false;
			}
		}

		public IEnumerable<TrainerViewModel> GetAllTrainers()
		{
			var Trainers = _unitOfWork.GetRepository<TrainerEntity>().GetAll();
			if (!Trainers.Any()) return [];

			var mappedTrainers = _mapper.Map<IEnumerable<TrainerEntity>, IEnumerable<TrainerViewModel>>(Trainers);
			return mappedTrainers;
		}

		public TrainerViewModel? GetTrainerDetails(int trainerId)
		{
			var Trainer = _unitOfWork.GetRepository<TrainerEntity>().GetById(trainerId);
			if (Trainer is null) return null;

			var mappedTrainer = _mapper.Map<TrainerEntity, TrainerViewModel>(Trainer);
			return mappedTrainer;
		}
		public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
		{
			var Trainer = _unitOfWork.GetRepository<TrainerEntity>().GetById(trainerId);
			if (Trainer is null) return null;

			var mappedTrainer = _mapper.Map<TrainerEntity, TrainerToUpdateViewModel>(Trainer);
			return mappedTrainer;



		}
		public bool RemoveTrainer(int trainerId)
		{
			var Repo = _unitOfWork.GetRepository<TrainerEntity>();
			var TrainerToRemove = Repo.GetById(trainerId);
			if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
			Repo.Delete(TrainerToRemove);
			return _unitOfWork.SaveChanges() > 0;

		}

		public bool UpdateTrainerDetails(UpdateTrainerViewModel updatedTrainer, int trainerId)
		{
			var Repo = _unitOfWork.GetRepository<TrainerEntity>();
			var TrainerToUpdate = Repo.GetById(trainerId);

			if (TrainerToUpdate is null || IsEmailUnique(updatedTrainer.Email) || IsPhoneUnique(updatedTrainer.Phone)) return false;

			_mapper.Map(updatedTrainer, TrainerToUpdate);
			TrainerToUpdate.UpdatedAt = DateTime.Now;

			return _unitOfWork.SaveChanges() > 0;
		}
		#region Helper Methods

		private string FormatAddress(Address address)
		{
			if (address == null) return "N/A";
			return $"{address.BuildingNumber} - {address.Street} - {address.City}";
		}
		private bool IsEmailUnique(string email)
		{
			var existing = _unitOfWork.GetRepository<MemberEntity>().GetAll(
				m => m.Email == email);
			return !existing.Any();
		}

		private bool IsPhoneUnique(string phone)
		{
			var existing = _unitOfWork.GetRepository<MemberEntity>().GetAll(
				m => m.Phone == phone);
			return !existing.Any();
		}

		private bool HasActiveSessions(int Id)
		{
			var activeSessions = _unitOfWork.GetRepository<SessionEntity>().GetAll(
			   s => s.TrainerId == Id && s.StartDate > DateTime.Now).Any();
			return activeSessions;
		}
		#endregion
	}
}
