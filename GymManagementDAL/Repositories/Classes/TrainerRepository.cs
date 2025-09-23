using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
	internal class TrainerRepository : ITrainerRepository
	{
		private readonly GymDbContext _dbContext;

		public TrainerRepository(GymDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public int Add(TrainerEntity trainer)
		{
			_dbContext.Add(trainer);
			return _dbContext.SaveChanges();
		}

		public int Delete(TrainerEntity trainer)
		{
			_dbContext.Remove(trainer);
			return _dbContext.SaveChanges();
		}

		public IEnumerable<TrainerEntity> GetAllAsync()
		=> _dbContext.Trainers.AsNoTracking().ToList();


		public TrainerEntity? GetByIdAsync(int id)
		=> _dbContext.Trainers.Find(id);


		public int Update(TrainerEntity trainer)
		{
			_dbContext.Update(trainer);
			return _dbContext.SaveChanges();
		}
	}
}
