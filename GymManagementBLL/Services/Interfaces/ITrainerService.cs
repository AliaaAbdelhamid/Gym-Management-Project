using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        bool CreateTrainer(CreateTrainerViewModel createTrainer);

        bool UpdateTrainerDetails(UpdateTrainerViewModel updatedTrainer,int trainerId);
        bool RemoveTrainer(int trainerId);

        TrainerViewModel? GetTrainerDetails(int trainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);


		IEnumerable<TrainerViewModel> GetAllTrainers();


    }
}
