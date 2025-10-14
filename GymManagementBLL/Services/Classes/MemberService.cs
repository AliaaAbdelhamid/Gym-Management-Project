using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class MemberService : IMemberService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public bool CreateMember(CreateMemberViewModel CreatedMember)
		{
			try
			{
				var Repo = _unitOfWork.GetRepository<MemberEntity>();

				if (IsEmailExists(CreatedMember.Email))
					return false;
				if (IsPhoneExists(CreatedMember.Phone))
					return false;
				var MemberEntity = _mapper.Map<MemberEntity>(CreatedMember);
				Repo.Add(MemberEntity);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch
			{
				return false;
			}


		}

		public IEnumerable<MemberViewModel> GetAllMembers()
		{
			var Members = _unitOfWork.GetRepository<MemberEntity>().GetAll();
			if (!Members.Any()) return [];
			return _mapper.Map<IEnumerable<MemberViewModel>>(Members);
		}

		public MemberViewModel? GetMemberDetails(int MemberId)
		{
			var member = _unitOfWork.GetRepository<MemberEntity>().GetById(MemberId);

			if (member is null) return null;

			var viewModel = _mapper.Map<MemberViewModel>(member);

			var activeMemberShip = _unitOfWork.GetRepository<MembershipEntity>()
				.GetAll(MP => MP.MemberId == MemberId && MP.Status == "Active").FirstOrDefault();

			if (activeMemberShip is not null)
			{
				var activePlan = _unitOfWork.GetRepository<PlanEntity>().GetById(activeMemberShip.PlanId);

				viewModel.PlanName = activePlan?.Name;
				viewModel.MembershipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
				viewModel.MembershipEndDate = activeMemberShip.EndDate.ToShortDateString();
			}

			return viewModel;
		}

		public HealthRecordViewModel? GetMemberHealthRecord(int MemberId)
		{
			var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecordEntity>().GetById(MemberId);
			if (MemberHealthRecord is null) return null;

			return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
		}

		public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
		{
			var member = _unitOfWork.GetRepository<MemberEntity>().GetById(MemberId);
			if (member is null) return null;
			return _mapper.Map<MemberToUpdateViewModel>(member);
		}

		public bool RemoveMember(int MemberId)
		{
			var Repo = _unitOfWork.GetRepository<MemberEntity>();
			var Member = Repo.GetById(MemberId);
			if (Member is null) return false;
			var sessionIds = _unitOfWork.GetRepository<BookingEntity>().GetAll(
			   b => b.MemberId == MemberId).Select(S => S.SessionId); // 1 5 8

			var hasFutureSessions = _unitOfWork.GetRepository<SessionEntity>()
				.GetAll(S => sessionIds.Contains(S.Id) && S.StartDate > DateTime.Now).Any();

			if (hasFutureSessions) return false;

			var MemberShips = _unitOfWork.GetRepository<MembershipEntity>().GetAll(X => X.MemberId == MemberId);

			try
			{
				if (MemberShips.Any())
				{
					foreach (var membership in MemberShips)
						_unitOfWork.GetRepository<MembershipEntity>().Delete(membership);
				}
				_unitOfWork.GetRepository<MemberEntity>().Delete(Member);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch
			{
				return false;
			}

		}

		public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
		{
			var emailExist = _unitOfWork.GetRepository<MemberEntity>().GetAll(
				m => m.Email == UpdatedMember.Email && m.Id != Id);

			var PhoneExist = _unitOfWork.GetRepository<MemberEntity>().GetAll(
				m => m.Phone == UpdatedMember.Phone && m.Id != Id);

			if (emailExist.Any() || PhoneExist.Any()) return false;

			var Repo = _unitOfWork.GetRepository<MemberEntity>();
			var Member = Repo.GetById(Id);
			if (Member is null) return false;
			_mapper.Map(UpdatedMember, Member);

			Repo.Update(Member);
			return _unitOfWork.SaveChanges() > 0;
		}

		#region Helper Methods


		private bool IsEmailExists(string email)
		{
			var existing = _unitOfWork.GetRepository<MemberEntity>().GetAll(
				m => m.Email == email);
			return existing.Any();
		}

		private bool IsPhoneExists(string phone)
		{
			var existing = _unitOfWork.GetRepository<MemberEntity>().GetAll(
				m => m.Phone == phone);
			return existing.Any();
		}
		#endregion
	}
}
