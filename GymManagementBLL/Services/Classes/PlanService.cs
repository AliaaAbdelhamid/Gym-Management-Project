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
	public class PlanService : IPlanService
	{
		private readonly IUnitOfWork _unitOfWork;

		public PlanService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<PlanViewModel> GetAllPlans()
		{
			var Plans = _unitOfWork.GetRepository<PlanEntity>().GetAll();

			return Plans.Select(P => new PlanViewModel()
			{
				Id=P.Id,
				Name = P.Name,
				Description = P.Description,
				DurationDays = P.DurationDays,
				Price = P.Price,
				IsActive = P.IsActive
			});
		}

		public PlanViewModel? GetPlanById(int planId)
		{
			var plan = _unitOfWork.GetRepository<PlanEntity>().GetById(planId);

			if (plan == null)
				return null;

			return new PlanViewModel
			{
				Id = plan.Id,
				Name = plan.Name,
				Description = plan.Description,
				DurationDays = plan.DurationDays,
				Price = plan.Price,
				IsActive = plan.IsActive
			};
		}

		public UpdatePlanViewModel? GetPlanToUpdate(int planId)
		{
			var plan = _unitOfWork.GetRepository<PlanEntity>().GetById(planId);

			if (plan == null || plan.IsActive == false)
				return null;

			return new UpdatePlanViewModel
			{
				PlanName = plan.Name,
				Description = plan.Description,
				DurationDays = plan.DurationDays,
				Price = plan.Price
			};
		}

		public bool Activate(int PlanId)
		{
			try
			{
				var Repo = _unitOfWork.GetRepository<PlanEntity>();
				var Plan = Repo.GetById(PlanId);
				if (Plan is null) return false;
				Plan.IsActive = Plan.IsActive == true ? false : true;
				Plan.UpdatedAt = DateTime.Now;
				Repo.Update(Plan);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch 
			{
				return false;
			}
		}

		public bool UpdatePlan(int Id, UpdatePlanViewModel updatePlanViewModel)
		{
			try
			{
				var Repo = _unitOfWork.GetRepository<PlanEntity>();
				var Plan = Repo.GetById(Id);
				if (Plan is null) return false;
				(Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt)
					= (updatePlanViewModel.Description, updatePlanViewModel.Price, updatePlanViewModel.DurationDays, DateTime.Now);
				Repo.Update(Plan);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch
			{
				return false;
			}
		}
	}
}
