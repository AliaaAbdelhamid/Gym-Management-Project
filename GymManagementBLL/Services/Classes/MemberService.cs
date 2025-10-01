using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
	public class MemberService : IMemberService
	{
		private readonly IUnitOfWork _unitOfWork;

		public MemberService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
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
				var MemberEntity = new MemberEntity()
				{
					Name = CreatedMember.Name,
					Email = CreatedMember.Email,
					Phone = CreatedMember.Phone,
					DateOfBirth = CreatedMember.DateOfBirth,
					Gender = CreatedMember.Gender,
					Address = new Address()
					{
						BuildingNumber = CreatedMember.BuildingNumber,
						City = CreatedMember.City,
						Street = CreatedMember.Street
					},
					HealthRecord = new HealthRecordEntity()
					{
						Note = CreatedMember.HealthRecordViewModel.Note,
						BloodType = CreatedMember.HealthRecordViewModel.BloodType,
						Height = CreatedMember.HealthRecordViewModel.Height,
						Weight = CreatedMember.HealthRecordViewModel.Weight
					}
				};
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
			var Members = _unitOfWork.GetRepository<MemberEntity>().GetAll() ?? [];
			if (!Members.Any()) return [];

			var memberViewModels = Members.Select(m => new MemberViewModel
			{
				Id = m.Id,
				Name = m.Name,
				Email = m.Email,
				Phone = m.Phone,
				DateOfBirth = m.DateOfBirth.ToShortDateString(),
				Gender = m.Gender.ToString(),
				Address = FormatAddress(m.Address),
				Photo = m.Photo
			});
			return memberViewModels;
		}

		public MemberViewModel? GetMemberDetails(int MemberId)
		{
			var member = _unitOfWork.GetRepository<MemberEntity>().GetById(MemberId);

			if (member is null) return null;

			var viewModel = new MemberViewModel
			{
				Id = member.Id,
				Name = member.Name,
				Email = member.Email,
				Phone = member.Phone,
				Gender = member.Gender.ToString(),
				DateOfBirth = member.DateOfBirth.ToShortDateString(),
				Address = FormatAddress(member.Address),
				Photo = member.Photo
			};

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

			return new HealthRecordViewModel()
			{
				BloodType = MemberHealthRecord.BloodType,
				Height = MemberHealthRecord.Height,
				Weight = MemberHealthRecord.Weight,
				Note = MemberHealthRecord.Note
			};
		}

		public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
		{
			var Member = _unitOfWork.GetRepository<MemberEntity>().GetById(MemberId);
			if (Member is null) return null;
			return new MemberToUpdateViewModel()
			{
				Email = Member.Email,
				Phone = Member.Phone,
				BuildingNumber = Member.Address.BuildingNumber,
				City = Member.Address.City,
				Street = Member.Address.Street,
				Name = Member.Name,
				Photo = Member.Photo,
			};

		}

		public bool RemoveMember(int MemberId)
		{
			var Repo = _unitOfWork.GetRepository<MemberEntity>();
			var Member = Repo.GetById(MemberId);
			if (Member is null) return false;
			var activeBookings = _unitOfWork.GetRepository<BookingEntity>().GetAll(
			   b => b.MemberId == MemberId && b.Session.StartDate > DateTime.UtcNow);

			if (activeBookings.Any()) return false;

			try
			{
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
			if (IsEmailExists(UpdatedMember.Email))
				return false;
			if (IsPhoneExists(UpdatedMember.Phone))
				return false;

			var Repo = _unitOfWork.GetRepository<MemberEntity>();
			var Member = Repo.GetById(Id);
			if (Member is null) return false;
			Member.Email = UpdatedMember.Email;
			Member.Phone = UpdatedMember.Phone;
			Member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
			Member.Address.City = UpdatedMember.City;
			Member.Address.Street = UpdatedMember.Street;
			Member.UpdatedAt = DateTime.Now;

			Repo.Update(Member);
			return _unitOfWork.SaveChanges() > 0;
		}

		public bool UpdateMemberHealthRecord(int Id, HealthRecordViewModel UpdatedHealthRecord)
		{
			var Repo = _unitOfWork.GetRepository<HealthRecordEntity>();
			var MemberHealthRecord = Repo.GetById(Id);
			if (MemberHealthRecord is null) return false;
			MemberHealthRecord.Weight = UpdatedHealthRecord.Weight;
			MemberHealthRecord.Height = UpdatedHealthRecord.Height;
			MemberHealthRecord.BloodType = UpdatedHealthRecord.BloodType;
			MemberHealthRecord.Note = UpdatedHealthRecord.Note;
			MemberHealthRecord.UpdatedAt = DateTime.Now;

			Repo.Update(MemberHealthRecord);
			return _unitOfWork.SaveChanges() > 0;

		}

		#region Helper Methods

		private string FormatAddress(Address address)
		{
			if (address == null) return "N/A";
			return $"{address.BuildingNumber} - {address.Street} - {address.City}";
		}
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
