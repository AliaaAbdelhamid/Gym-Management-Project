using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
	internal class PlanService : IPlanService
	{
		private readonly IUnitOfWork _unitOfWork;

		public PlanService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<PlanViewModel> GetAllPlans()
		{
			var Plans = _unitOfWork.GetRepository<PlanEntity>().GetAll(X => X.IsActive == true);

			return Plans.Select(P => new PlanViewModel()
			{
				Name = P.Name,
				Description = P.Description,
				DurationDays = P.DurationDays,
				Price = P.Price,
				IsActive = P.IsActive
			});
		}

		public bool RemovePlan(int PlanId)
		{
			var Repo = _unitOfWork.GetRepository<PlanEntity>();
			var Plan = Repo.GetById(PlanId);
			if (Plan is null) return false;
			Plan.IsActive = false; // Soft Delete
			Plan.UpdatedAt = DateTime.Now;
			Repo.Update(Plan);
			return _unitOfWork.SaveChanges() > 0;
		}

		public bool UpdatePlan(int Id, UpdatePlanViewModel updatePlanViewModel)
		{
			var Repo = _unitOfWork.GetRepository<PlanEntity>();
			var Plan = Repo.GetById(Id);
			if (Plan is null) return false;
			(Plan.Description, Plan.Price, Plan.DurationDays , Plan.UpdatedAt) 
				= (updatePlanViewModel.Description, updatePlanViewModel.Price, updatePlanViewModel.DurationDays , DateTime.Now);
			Repo.Update(Plan);
			return _unitOfWork.SaveChanges() > 0;
		}
	}
}
