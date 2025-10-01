using AutoMapper;
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
			CreateMap<UpdateSessionViewModel, SessionEntity>();
			CreateMap<SessionEntity, SessionViewModel>();
			CreateMap<SessionEntity, UpdateSessionViewModel>();
			CreateMap<TrainerEntity, TrainerSelectViewModel>();
			CreateMap<CategoryEntity, CategorySelectViewModel>()
				.ForMember(dist => dist.Name, opt => opt.MapFrom(src => src.CategoryName));
		}

	}
}
