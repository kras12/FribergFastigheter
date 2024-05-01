using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting view model classes to other view model classes classes.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class ViewModelToViewModelAutoMapperProfile : Profile
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        public ViewModelToViewModelAutoMapperProfile()
		{
            CreateMap<HousingViewModel, EditHousingViewModel>()
                .ForMember(x => x.BrokerFirmId, opt => opt.MapFrom(src => src.Broker.BrokerFirm.BrokerFirmId))
                .ForMember(x => x.BrokerId, opt => opt.MapFrom(src => src.Broker.BrokerId))
                .ForMember(x => x.SelectedMunicipalityId, opt => opt.MapFrom(src => src.Municipality.MunicipalityId))
                .ForMember(x => x.SelectedCategoryId, opt => opt.MapFrom(src => src.Category.HousingCategoryId))
                .ReverseMap();
        }		
	}
}
