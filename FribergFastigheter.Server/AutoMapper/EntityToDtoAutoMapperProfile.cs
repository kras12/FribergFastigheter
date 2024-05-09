using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Server.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting entity classes to DTO classes.
	/// </summary>
	/// <!-- Author: Jimmie, Marcus -->
	/// <!-- Co Authors: -->
	public class EntityToDtoAutoMapperProfile : Profile
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityToDtoAutoMapperProfile()
		{
			CreateMap<Housing, HousingDto>();
			CreateMap<HousingCategory, HousingCategoryDto>();
			CreateMap<Municipality, MunicipalityDto>();
			CreateMap<Image, ImageDto>();
            CreateMap<ApplicationUser, CreatedBrokerDto>();

            CreateMap<BrokerFirm, BrokerFirmDto>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<BrokerFirm, BrokerFirmSummaryDto>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<Broker, BrokerDto>()
				.ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(x => x.User.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(x => x.User.LastName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.User.Email))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(x => x.User.PhoneNumber));

        }		
	}
}
