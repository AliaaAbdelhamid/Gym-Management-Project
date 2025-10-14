using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
	public class BookingController : Controller
	{
		private readonly IBookingService _bookingService;

		public BookingController(IBookingService bookingService)
		{
			_bookingService = bookingService;
		}
		public IActionResult Index()
		{
			var Bookings = _bookingService.GetAllSessions();
			return View(Bookings);
		}
		public ActionResult GetMembersForUpcomingSession(int id)
		{
			var Members = _bookingService.GetMembersForUpcomingBySessionId(id);
			return View(Members);
		}
		public ActionResult Create(int id)
		{
			var members = _bookingService.GetMembersForDropDown(id);
			ViewBag.members = new SelectList(members, "Id", "Name");
			return View();
		}
		[HttpPost]
		public ActionResult Create(CreateBookingViewModel createdBooking)
		{

			var result = _bookingService.CreateNewBooking(createdBooking);
			if (result)
			{
				TempData["SuccessMessage"] = "Booking Created successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to Create Booking.";
			}

			return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = createdBooking.SessionId });


		}
		[HttpPost]
		public ActionResult Cancel(int MemberId, int SessionId)
		{
			var result = _bookingService.CancelBooking(MemberId, SessionId);
			if (result)
			{
				TempData["SuccessMessage"] = "Booking cancelled successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to cancel Booking.";
			}

			return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = SessionId });
		}
		public ActionResult GetMembersForOngoingSessions(int id)
		{
			var Members = _bookingService.GetMembersForOngoingBySessionId(id);
			return View(Members);
		}
		[HttpPost]
		public ActionResult Attended(int MemberId, int SessionId)
		{
			var result = _bookingService.MemberAttended(MemberId, SessionId);
			return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = SessionId });

		}
	}
}
