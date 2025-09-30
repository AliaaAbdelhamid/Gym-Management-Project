using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

				var MemberExist = Repo.GetAll(X => X.Email == CreatedMember.Email || X.Phone == CreatedMember.Phone).Any();
				if (MemberExist) return false;

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
				Name = m.Name,
				Email = m.Email,
				Phone = m.Phone,
				DateOfBirth = m.DateOfBirth.ToShortDateString(),
				Gender = m.Gender.ToString(),
				Address = $"{m.Address.BuildingNumber} - {m.Address.Street} -{m.Address.City}",
				Photo = m.Photo
			});
			return memberViewModels;
		}

		public MemberViewModel? GetMemberDetails(int MemberId)
		{
			var Member = _unitOfWork.GetRepository<MemberEntity>().GetById(MemberId);

			if (Member is null) return null;

			return new MemberViewModel()
			{
				Name = Member.Name,
				Email = Member.Email,
				Phone = Member.Phone,
				Gender = Member.Gender.ToString(),
				DateOfBirth = Member.DateOfBirth.ToShortDateString(),
				Address = $"{Member.Address.BuildingNumber} - {Member.Address.Street} - {Member.Address.City}",
				Photo = Member.Photo
			};
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

		public bool RemoveMember(int MemberId)
		{
			var Repo = _unitOfWork.GetRepository<MemberEntity>();
			var Member = Repo.GetById(MemberId);
			if (Member is null) return false;

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

		public bool UpdateMemberDetails( int Id ,  UpdateMemberViewModel UpdatedMember)
		{
			var Repo = _unitOfWork.GetRepository<MemberEntity>();
			var Member = Repo.GetById(Id);
			if(Member is null) return false;
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
	}
}
