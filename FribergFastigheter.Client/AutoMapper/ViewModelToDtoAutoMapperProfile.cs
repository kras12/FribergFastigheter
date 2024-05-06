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
        public ViewModelToDtoAutoMapperProfile()
		{
            CreateMap<EditHousingViewModel, EditHousingDto>()
            .ForMember(dest => dest.BrokerId, opt => opt.MapFrom(src => src.BrokerId))
            .ForMember(dest => dest.BrokerFirmId, opt => opt.MapFrom(src => src.BrokerFirmId))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategoryId))
            .ForMember(dest => dest.MunicipalityId, opt => opt.MapFrom(src => src.SelectedMunicipalityId));

            CreateMap<CreateHousingViewModel, CreateHousingDto>()
            .ForMember(dest => dest.BrokerId, opt => opt.MapFrom(src => src.BrokerId))
            .ForMember(dest => dest.BrokerFirmId, opt => opt.MapFrom(src => src.BrokerFirmId))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategoryId))
            .ForMember(dest => dest.MunicipalityId, opt => opt.MapFrom(src => src.SelectedMunicipalityId));

            CreateMap<CreateBrokerViewModel, RegisterBrokerDto>()
                .ForMember(dest => dest.BrokerFirmId, opt => opt.MapFrom(src => src.BrokerFirmId));

            CreateMap<HousingViewModel, HousingDto>();
            CreateMap<HousingCategoryViewModel, HousingCategoryDto>();
            CreateMap<BrokerViewModel,  BrokerDto>();
            CreateMap<BrokerFirmViewModel,  BrokerFirmDto>();
            CreateMap<MunicipalityViewModel, MunicipalityDto>();
            CreateMap<BrokerFirmSummaryViewModel, BrokerFirmSummaryDto>();
            CreateMap<ImageViewModel,  ImageDto>();

            CreateMap<EditBrokerViewModel, EditBrokerDto>()
                .ForMember(dest => dest.BrokerFirmId, opt => opt.MapFrom(src => src.BrokerFirm.BrokerFirmId));
        }		
	}
}
