using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting view model classes to DTO classes.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class ViewModelToDtoAutoMapperProfile : Profile
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        public ViewModelToDtoAutoMapperProfile()
		{
            // TODO - Likely obsolete and can be deleted later
            //CreateMap<HousingViewModel, HousingDto>();
            //CreateMap<HousingCategoryViewModel, HousingCategoryDto>();
            //CreateMap<MunicipalityViewModel, MunicipalityDto>();
            //CreateMap<ImageViewModel, ImageDto>();
            //CreateMap<BrokerFirmViewModel, BrokerFirmDto>()
            //    .ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));
            //CreateMap<BrokerViewModel, BrokerDto>()
			//	.ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null));

            CreateMap<UpdateHousingViewModel, UpdateHousingDto>()
            .ForMember(dest => dest.BrokerId, opt => opt.MapFrom(src => src.BrokerId))
            .ForMember(dest => dest.BrokerFirmId, opt => opt.MapFrom(src => src.BrokerFirmId))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.MunicipalityId, opt => opt.MapFrom(src => src.MunicipalityId));

            CreateMap<CreateHousingViewModel, CreateHousingDto>()
            .ForMember(dest => dest.BrokerId, opt => opt.MapFrom(src => src.BrokerId))
            .ForMember(dest => dest.BrokerFirmId, opt => opt.MapFrom(src => src.BrokerFirmId))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.MunicipalityId, opt => opt.MapFrom(src => src.MunicipalityId));
        }		
	}
}
