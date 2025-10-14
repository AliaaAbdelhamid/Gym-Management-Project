using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interfaces
{
	public interface IBookingService
	{
		IEnumerable<SessionViewModel> GetAllSessions();
		IEnumerable<MemberForSessionViewModel> GetMembersForUpcomingBySessionId(int sessionId);
		IEnumerable<MemberForSessionViewModel> GetMembersForOngoingBySessionId(int sessionId);
		IEnumerable<MemberSelectListViewModel> GetMembersForDropDown(int sessionId);
		bool CancelBooking(int MemberId, int SessionId);
		bool CreateNewBooking(CreateBookingViewModel createdBooking);
		bool MemberAttended(int MemberId, int SessionId);
	}
}
