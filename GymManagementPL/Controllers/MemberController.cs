using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementPL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
		#region Get All Members
		public IActionResult Index()
		{
			var Members = _memberService.GetAllMembers();
			return View(Members);
		}
		#endregion

		#region Create Member 

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult CreateMember(CreateMemberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("DataMissed", "Check Missing Fields");
				return View(nameof(Create), model);
			}

			bool Result =  _memberService.CreateMember(model);
			if (Result)
			{
				TempData["SuccessMessage"] = "Member Updated Successfully.";
			}
			else
			{
				TempData["ErrorMessage"] = "Member Failed To Create , Phone Number Or Email already exists";
			}
			return RedirectToAction(nameof(Index));

		}
		#endregion
		
		#region Show Member Data 

		public IActionResult MemberDetails(int id)
		{
			var member = _memberService.GetMemberDetails(id);

			if (member == null)
			{
				TempData["ErrorMessage"] = "Member not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(member);
		}
		public IActionResult HealthRecordDetails(int id)
		{
			var member = _memberService.GetMemberHealthRecord(id);

			if (member == null)
			{
				TempData["ErrorMessage"] = "Member not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(member);
		} 
		#endregion

		#region Member Data Edit 
		public IActionResult MemberEdit(int id)
		{
			var member = _memberService.GetMemberToUpdate(id);

			if (member == null)
			{
				TempData["ErrorMessage"] = "Member not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(member);
		}

		[HttpPost]
		public IActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel memberToUpdate)
		{
			if (!ModelState.IsValid)
			{
				return View(memberToUpdate);
			}

			var result = _memberService.UpdateMemberDetails(id, memberToUpdate);

			if (result)
			{
				TempData["SuccessMessage"] = "Member Updated Successfully.";
			}
			else
			{
				TempData["ErrorMessage"] = "Member Failed To Update .";
			}
			return RedirectToAction(nameof(Index));
		}
		#endregion

		#region Delete Member 

		public IActionResult Delete([FromRoute]int id)
		{
			var member =  _memberService.GetMemberDetails(id);

			if (member == null)
			{
				TempData["ErrorMessage"] = "Member not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(member);
		}

		[HttpPost]
		public IActionResult DeleteConfirmed([FromForm] int id)
		{
			var Result = _memberService.RemoveMember(id);

			if (Result)
				TempData["SuccessMessage"] = "Member deleted successfully!";
			else
				TempData["ErrorMessage"] = "Member not found.";
			return RedirectToAction(nameof(Index));
		}

		#endregion
	}
}
