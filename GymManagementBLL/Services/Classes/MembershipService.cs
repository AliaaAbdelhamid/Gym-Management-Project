using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class MembershipService : IMembershipService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public bool CreateMembership(CreateMemberShipViewModel CreatedMemberShip)
		{
			try
			{
				if (!IsMemberExists(CreatedMemberShip.MemberId) || !IsPlanExists(CreatedMemberShip.PlanId)
					|| HasActiveMemberShip(CreatedMemberShip.MemberId)) return false;
				var MemberShipToCreate = _mapper.Map<MembershipEntity>(CreatedMemberShip);
				var Plan = _unitOfWork.GetRepository<PlanEntity>().GetById(CreatedMemberShip.PlanId);
				MemberShipToCreate.EndDate = DateTime.Now.AddDays(Plan!.DurationDays);
				_unitOfWork.MembershipRepository.Add(MemberShipToCreate);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch
			{
				return false;
			}
		}
		public bool DeleteMemberShip(int MemberId)
		{
			var Repo = _unitOfWork.MembershipRepository;
			var ActiveMemberships = Repo.GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();
			if (ActiveMemberships is null) return false;
			Repo.Delete(ActiveMemberships);
			return _unitOfWork.SaveChanges() > 0;
		}
		public IEnumerable<MemberShipViewModel> GetAllMemberShips()
		{
			var MemberShips = _unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlan(X => X.Status == "Active");
			if (!MemberShips.Any()) return [];
			return _mapper.Map<IEnumerable<MemberShipViewModel>>(MemberShips);
		}
		public IEnumerable<PlanSelectListViewModel> GetPlansForDropDown()
		{
			var Plans = _unitOfWork.GetRepository<PlanEntity>().GetAll(X => X.IsActive == true);
			return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(Plans);
		}
		public IEnumerable<MemberSelectListViewModel> GetMembersForDropDown()
		{
			var Members = _unitOfWork.GetRepository<MemberEntity>().GetAll();
			return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(Members);
		}

		#region Helper Methods 

		private bool IsMemberExists(int MemberId)
		{
			return _unitOfWork.GetRepository<MemberEntity>().Exists(X => X.Id == MemberId);
		}
		private bool IsPlanExists(int PlanId)
		{
			return _unitOfWork.GetRepository<PlanEntity>().Exists(X => X.Id == PlanId);
		}
		private bool HasActiveMemberShip(int memberId)
		{
			return _unitOfWork.MembershipRepository.Exists(X => X.MemberId == memberId && X.Status == "Active");
		}


		#endregion
	}
}
