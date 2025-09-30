using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
	internal interface ITrainerRepository
	{
		TrainerEntity? GetById(int id);
		IEnumerable<TrainerEntity> GetAll();
		int Add(TrainerEntity trainer);
		int Update(TrainerEntity trainer);
		int Delete(TrainerEntity trainer);
	}
}
