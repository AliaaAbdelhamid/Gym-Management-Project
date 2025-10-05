using GymManagementBLL.ViewModels.PlanViewModels;

namespace GymManagementBLL.Services.Interfaces
{
	public interface IPlanService
	{
		bool UpdatePlan(int Id, UpdatePlanViewModel updatePlanViewModel);
		UpdatePlanViewModel? GetPlanToUpdate(int planId);
		IEnumerable<PlanViewModel> GetAllPlans();
		PlanViewModel? GetPlanById(int planId);
		bool Activate(int PlanId);
	}
}
