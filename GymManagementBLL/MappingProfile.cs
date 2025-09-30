using AutoMapper;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfile:Profile
    {

        public MappingProfile()
        {
            MapTrainer();          
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

    }
}
