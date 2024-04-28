using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting DTO classes to view model classes.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class DtoToViewModelAutoMapperProfile : Profile
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public DtoToViewModelAutoMapperProfile()
		{
			CreateMap<HousingDto, HousingViewModel>();
			CreateMap<HousingCategoryDto, HousingCategoryViewModel>();
			CreateMap<MunicipalityDto, MunicipalityViewModel>();
			CreateMap<ImageDto, ImageViewModel>();
			CreateMap<PaginationDto, PaginationViewModel>();

			CreateMap<BrokerFirmSummaryDto, BrokerFirmViewModel>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<BrokerDto, BrokerViewModel>()
				.ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null));

            CreateMap<HousingSearchResultDto, HousingSearchResultViewModel>()
				.ForMember(dest => dest.Pagination, opt => opt.PreCondition(x => x.Pagination != null));
        }
	}
}
