using AutoMapper;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;

namespace GymManagementBLL
{
	public class MappingProfile : Profile
	{

		public MappingProfile()
		{
			MapTrainer();
			MapSession();
			MapMemberships();
			MapMember();
			MapPlan();
		}

		private void MapTrainer()
		{
			CreateMap<CreateTrainerViewModel, TrainerEntity>()
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
				{
					BuildingNumber = src.BuildingNumber,
					Street = src.Street,
					City = src.City
				}));
			CreateMap<TrainerEntity, TrainerViewModel>();
			CreateMap<TrainerEntity, TrainerToUpdateViewModel>()
				.ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
				.ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
				.ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

			CreateMap<UpdateTrainerViewModel, TrainerEntity>()
			.ForMember(dest => dest.Name, opt => opt.Ignore())
			.AfterMap((src, dest) =>
			{
				dest.Address.BuildingNumber = src.BuildingNumber;
				dest.Address.City = src.City;
				dest.Address.Street = src.Street;
				dest.UpdatedAt = DateTime.Now;
			});
		}
		private void MapSession()
		{
			CreateMap<CreateSessionViewModel, SessionEntity>();
			CreateMap<SessionEntity, SessionViewModel>()
						.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
						.ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
						.ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()); // Will Be Calculated After Map
			CreateMap<UpdateSessionViewModel, SessionEntity>().ReverseMap();


			CreateMap<TrainerEntity, TrainerSelectViewModel>();
			CreateMap<CategoryEntity, CategorySelectViewModel>()
				.ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.CategoryName));
		}
		private void MapMember()
		{
			CreateMap<CreateMemberViewModel, MemberEntity>()
				  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
				  {
					  BuildingNumber = src.BuildingNumber,
					  City = src.City,
					  Street = src.Street
				  })).ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


			CreateMap<HealthRecordViewModel, HealthRecordEntity>().ReverseMap();
			CreateMap<MemberEntity, MemberViewModel>()
		   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
			.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
			.ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

			CreateMap<MemberEntity, MemberToUpdateViewModel>()
			.ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
			.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
			.ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

			CreateMap<MemberToUpdateViewModel, MemberEntity>()
				.ForMember(dest => dest.Name, opt => opt.Ignore())
				.ForMember(dest => dest.Photo, opt => opt.Ignore())
				.AfterMap((src, dest) =>
				{
					dest.Address.BuildingNumber = src.BuildingNumber;
					dest.Address.City = src.City;
					dest.Address.Street = src.Street;
					dest.UpdatedAt = DateTime.Now;
				});
		}
		private void MapPlan()
		{
			CreateMap<PlanEntity, PlanViewModel>();
			CreateMap<PlanEntity, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
			CreateMap<UpdatePlanViewModel, PlanEntity>()
		   .ForMember(dest => dest.Name, opt => opt.Ignore())
		   .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

		}
		private void MapMemberships()
		{
			CreateMap<MembershipEntity, MemberShipForMemberViewModel>()
					 .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
					 .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
					 .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

			CreateMap<MembershipEntity, MemberShipViewModel>()
					 .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
					 .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
					 					 .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

			CreateMap<CreateMemberShipViewModel, MembershipEntity>();
			CreateMap<MemberEntity, MemberSelectListViewModel>();
			CreateMap<PlanEntity, PlanSelectListViewModel>();
		}
	}
}
