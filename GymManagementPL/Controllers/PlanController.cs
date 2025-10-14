using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	public class PlanController : Controller
	{
		private readonly IPlanService _planService;

		public PlanController(IPlanService planService)
		{
			_planService = planService;
		}

		#region Get All Plans
		public IActionResult Index()
		{
			var plans = _planService.GetAllPlans();
			return View(plans);
		}
		#endregion

		#region Plan Details
		public IActionResult Details(int id)
		{
			var plan = _planService.GetPlanById(id);

			if (plan == null)
			{
				TempData["ErrorMessage"] = "Plan not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(plan);
		}
		#endregion

		#region Edit Plan
		public IActionResult Edit(int id)
		{
			var plan = _planService.GetPlanToUpdate(id);

			if (plan == null)
			{
				TempData["ErrorMessage"] = "Plan Can not be Updated";
				return RedirectToAction(nameof(Index));
			}

			return View(plan);
		}

		[HttpPost]
		public IActionResult Edit([FromRoute] int id, UpdatePlanViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = _planService.UpdatePlan(id, model);

			if (result)
			{
				TempData["SuccessMessage"] = "Plan updated successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to update plan.";
			}
			return RedirectToAction(nameof(Index));
		}
		#endregion

		#region Delete Plan

		[HttpPost]
		public IActionResult Activate(int id)
		{
			var result = _planService.Activate(id);

			if (result)
			{
				TempData["SuccessMessage"] = "Plan deactivated successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to deactivate plan.";
			}
			return RedirectToAction(nameof(Index));
		}
		#endregion
	}
}