using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

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
			if (Plans is null || !Plans.Any()) return [];

			return Plans.Select(P => new PlanViewModel()
			{
				Id = P.Id,
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
				if (Plan is null || HasActiveMemberShips(PlanId)) return false;
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
				if (Plan is null || HasActiveMemberShips(Id)) return false;
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

		#region Helper Methods
		private bool HasActiveMemberShips(int Id)
		{
			var activeMemberships = _unitOfWork.GetRepository<MembershipEntity>().GetAll(m => m.PlanId == Id && m.Status == "Active");
			if (activeMemberships.Any())
				return true;
			else
				return false;
		}
		#endregion
	}
}
