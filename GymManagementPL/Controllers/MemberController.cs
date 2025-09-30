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
        public IActionResult Index()
        {
            var model = new MemberIndexViewModel
            {
                Members = _memberService.GetAllMembers()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateMemberViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OpenCreateModal = true;
                return View("Index", new MemberIndexViewModel
                {
                    Members = _memberService.GetAllMembers(),
                    CreateMember = viewModel
                });
            }

            var result = _memberService.CreateMember(viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // If saving failed
            ModelState.AddModelError("savingfailed", "Something went wrong while saving.");
            ViewBag.OpenCreateModal = true;

            return View("Index", new MemberIndexViewModel
            {
                Members = _memberService.GetAllMembers(),
                CreateMember = viewModel
            });
        }

        [HttpPost]
        public IActionResult EditMemberData([FromRoute]int id ,UpdateMemberViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OpenEditMemberDetailsModal = id; // reopen correct modal
                return View("Index", new MemberIndexViewModel
                {
                    Members = _memberService.GetAllMembers(),
                    UpdateMemberDetails = viewModel
                });

            }
            var result = _memberService.UpdateMemberDetails(id , viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("savingfailed", "Something went wrong while saving.");
            ViewBag.OpenEditMemberDetailsModal = id; // reopen correct modal

            return View("Index", new MemberIndexViewModel
            {
                Members = _memberService.GetAllMembers(),
                UpdateMemberDetails = viewModel
            });
        }
    }
}
