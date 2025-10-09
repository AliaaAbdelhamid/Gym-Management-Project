using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
	public class MembershipController : Controller
	{
		private readonly IMembershipService _membershipService;

		public MembershipController(IMembershipService membershipService)
		{
			_membershipService = membershipService;
		}
		public IActionResult Index()
		{
			var memberships = _membershipService.GetAllMemberShips();
			return View(memberships);
		}
		public IActionResult MemberMemberships(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}

			var memberships = _membershipService.GetMembershipsForSpecificMember(id);

			if (!memberships.Any())
			{
				TempData["Warning"] = "No memberships found for this member.";
			}


			return View(memberships);
		}
		public IActionResult Create()
		{
			LoadDropdowns();
			return View();
		}
		[HttpPost]
		public IActionResult Create(CreateMemberShipViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = _membershipService.CreateMembership(model);

				if (result)
				{
					TempData["Success"] = "Membership created successfully!";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					TempData["Error"] = "Failed to create membership. member doesn't have an active membership.";
				}
			}
			LoadDropdowns();
			return View(model);
		}

		[HttpPost]
		public IActionResult RenewConfirmed(int memberId, int planId)
		{
			var result = _membershipService.RenewMemberShip(memberId, planId);

			if (result)
			{
				TempData["Success"] = "Membership renewed successfully!";
				return RedirectToAction(nameof(MemberMemberships), new { id = memberId });
			}
			else
			{
				TempData["Error"] = "Failed to renew membership. Please check if member and plan exist.";
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		public IActionResult Cancel(int id)
		{
			var result = _membershipService.DeleteMemberShip(id);

			if (result)
			{
				TempData["Success"] = "Membership cancelled successfully!";
			}
			else
			{
				TempData["Error"] = "Failed to cancel membership.";
			}

			return RedirectToAction(nameof(Index));
		}

		#region Helper Methods
		private void LoadDropdowns()
		{
			var members = _membershipService.GetMembersForDropDown();
			var plans = _membershipService.GetPlansForDropDown();

			ViewBag.members = new SelectList(members, "Id", "Name");
			ViewBag.plans = new SelectList(plans, "Id", "Name");
		}
		#endregion

	}
}
