using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class PlanService : IPlanService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public IEnumerable<PlanViewModel> GetAllPlans()
		{
			var plans = _unitOfWork.GetRepository<PlanEntity>().GetAll();
			if (!plans.Any()) return [];
			return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
		}
		public PlanViewModel? GetPlanById(int planId)
		{
			var plan = _unitOfWork.GetRepository<PlanEntity>().GetById(planId);

			if (plan == null)
				return null;

			return _mapper.Map<PlanViewModel>(plan);
		}
		public UpdatePlanViewModel? GetPlanToUpdate(int planId)
		{
			var plan = _unitOfWork.GetRepository<PlanEntity>().GetById(planId);

			if (plan == null || plan.IsActive == false || HasActiveMemberShips(planId))
				return null;

			return _mapper.Map<UpdatePlanViewModel>(plan);
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
				_mapper.Map(updatePlanViewModel, Plan);
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
