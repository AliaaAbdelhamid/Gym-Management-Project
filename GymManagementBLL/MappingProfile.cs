using AutoMapper;
using GymManagementBLL.ViewModels.MembershipViewModels;
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

			CreateMap<UpdateTrainerViewModel, TrainerEntity>().ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
			{
				BuildingNumber = src.BuildingNumber,
				Street = src.Street,
				City = src.City
			}));
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
