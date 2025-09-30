using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
	public interface IPlanService
	{
		bool UpdatePlan(int Id, UpdatePlanViewModel updatePlanViewModel);
		bool RemovePlan(int PlanId);
		IEnumerable<PlanViewModel> GetAllPlans();
	}
}
