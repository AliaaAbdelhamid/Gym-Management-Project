using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<TrainerEntity>();

                var TrainerExist = Repo.GetAll(X => X.Email == createTrainer.Email || X.Phone == createTrainer.Phone).Any();
                if (TrainerExist) return false;

                var TrainerEntity = _mapper.Map<CreateTrainerViewModel, TrainerEntity>(createTrainer);


                Repo.Add(TrainerEntity);

                return _unitOfWork.SaveChanges() > 0;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<TrainerEntity>().GetAll();
            if (!Trainers.Any()) return [];

            var mappedTrainers=_mapper.Map<IEnumerable<TrainerEntity>,IEnumerable<TrainerViewModel>>(Trainers);
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
            if (TrainerToRemove is null) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;

        }

        public bool UpdateTrainerDetails(UpdateTrainerViewModel updatedTrainer, int trainerId)
        {
            var Repo=_unitOfWork.GetRepository<TrainerEntity>();
            var TrainerToUpdate=Repo.GetById(trainerId);

            if(TrainerToUpdate is null) return false;

            _mapper.Map(updatedTrainer, TrainerToUpdate);
            TrainerToUpdate.UpdatedAt = DateTime.Now;

            return _unitOfWork.SaveChanges() > 0;
        }
    }
}
