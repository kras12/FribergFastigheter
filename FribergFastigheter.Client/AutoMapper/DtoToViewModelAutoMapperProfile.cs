using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.BrokerFirm;
using FribergFastigheter.Shared.Dto.Housing;
using FribergFastigheter.Shared.Dto.Image;
using FribergFastigheter.Shared.Dto.Statistics;

namespace FribergFastigheter.Client.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting DTO classes to view model classes.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: Marcus -->
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
            CreateMap<BrokerFirmStatisticsDto, BrokerFirmStatisticsViewModel>();
            CreateMap<StatisticItemDto, StatisticItemViewModel>();

            CreateMap<BrokerFirmDto, BrokerFirmViewModel>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<BrokerFirmSummaryDto, BrokerFirmSummaryViewModel>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<BrokerDto, BrokerViewModel>()
				.ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null));

            CreateMap<HousingSearchResultDto, HousingSearchResultViewModel>()
				.ForMember(dest => dest.Pagination, opt => opt.PreCondition(x => x.Pagination != null));
        }
	}
}
