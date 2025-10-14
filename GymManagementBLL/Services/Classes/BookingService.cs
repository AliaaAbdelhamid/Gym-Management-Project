using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class BookingService : IBookingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public bool CancelBooking(int MemberId, int SessionId)
		{
			try
			{
				var session = _unitOfWork.SessionRepository.GetById(SessionId);
				if (session is null || session.StartDate <= DateTime.Now) return false;

				var Booking = _unitOfWork.BookingRepository.GetAll(X => X.SessionId == SessionId && X.MemberId == MemberId)
														   .FirstOrDefault();
				if (Booking is null) return false;
				_unitOfWork.BookingRepository.Delete(Booking);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch
			{
				return false;
			}
		}
		public bool CreateNewBooking(CreateBookingViewModel createdBooking)
		{
			try
			{
				var session = _unitOfWork.SessionRepository.GetById(createdBooking.SessionId);
				if (session is null || session.StartDate <= DateTime.Now) return false;

				var HasActiveMembership = _unitOfWork.MembershipRepository.GetAll(X => X.MemberId == createdBooking.MemberId && X.Status == "Active").Any();
				if (!HasActiveMembership) return false;

				var HasAvailableSolts = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(createdBooking.SessionId);
				if (HasAvailableSolts == 0) return false;
				_unitOfWork.BookingRepository.Add(new BookingEntity()
				{
					MemberId = createdBooking.MemberId,
					SessionId = createdBooking.SessionId,
					IsAttended = false
				});

				return _unitOfWork.SaveChanges() > 0;
			}
			catch
			{
				return false;
			}
		}
		public IEnumerable<SessionViewModel> GetAllSessions()
		{
			var bookings = _unitOfWork.SessionRepository
				.GetAllSessionsWithTrainerAndCategory(X => X.EndDate >= DateTime.Now)
				.OrderByDescending(X => X.StartDate);

			if (!bookings.Any()) return null!;
			var MappedSession = _mapper.Map<IEnumerable<SessionViewModel>>(bookings);
			foreach (var item in MappedSession)
			{
				item.AvailableSlots = item.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(item.Id);
			}
			return MappedSession;
		}
		public IEnumerable<MemberForSessionViewModel> GetMembersForUpcomingBySessionId(int sessionId)
		{
			var MemberForSession = _unitOfWork.BookingRepository.GetBySessionId(sessionId);
			return MemberForSession.Select(X => new MemberForSessionViewModel
			{
				MemberId = X.MemberId,
				SessionId = sessionId,
				MemberName = X.Member.Name,
				BookingDate = X.CreatedAt.ToString()
			});
		}
		public IEnumerable<MemberForSessionViewModel> GetMembersForOngoingBySessionId(int sessionId)
		{
			var MemberForSession = _unitOfWork.BookingRepository.GetBySessionId(sessionId);
			return MemberForSession.Select(X => new MemberForSessionViewModel
			{
				MemberId = X.MemberId,
				SessionId = sessionId,
				MemberName = X.Member.Name,
				BookingDate = X.CreatedAt.ToString(),
				IsAttended = X.IsAttended
			});
		}
		public IEnumerable<MemberSelectListViewModel> GetMembersForDropDown(int sessionId)
		{
			var bookedMemberIds = _unitOfWork.GetRepository<BookingEntity>()
								   .GetAll(x => x.SessionId == sessionId)
								   .Select(x => x.MemberId)
								   .ToList();

			var availableMembers = _unitOfWork.GetRepository<MemberEntity>()
											  .GetAll(x => !bookedMemberIds.Contains(x.Id));

			return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(availableMembers);
		}
		public bool MemberAttended(int MemberId, int SessionId)
		{
			try
			{
				var memberSession = _unitOfWork.GetRepository<BookingEntity>()
										   .GetAll(X => X.MemberId == MemberId && X.SessionId == SessionId)
										   .FirstOrDefault();
				if (memberSession is null) return false;

				memberSession.IsAttended = true;
				memberSession.UpdatedAt = DateTime.Now;
				_unitOfWork.GetRepository<BookingEntity>().Update(memberSession);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch
			{
				return false;
			}
		}
	}
}
