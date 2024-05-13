using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.BrokerFirm;
using FribergFastigheter.Shared.Dto.Housing;
using FribergFastigheter.Shared.Dto.Image;
namespace FribergFastigheter.Server.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting DTO classes to entity classes.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class DtoToEntityAutoMapperProfile : Profile
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public DtoToEntityAutoMapperProfile()
		{
			CreateMap<HousingDto, Housing>();
			CreateMap<HousingCategoryDto, HousingCategory>();
			CreateMap<MunicipalityDto, Municipality>();
			CreateMap<ImageDto, Image>();

			CreateMap<BrokerFirmSummaryDto, BrokerFirm>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<BrokerDto, Broker>()
				.ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null));

			CreateMap<EditHousingDto, Housing>()
			.ForMember(dest => dest.Broker, opt => opt.MapFrom(src => new Broker() { BrokerId = src.BrokerId }))
			.ForMember(dest => dest.Category, opt => opt.MapFrom(src => new HousingCategory() { HousingCategoryId = src.CategoryId }))
			.ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => new Municipality() { MunicipalityId = src.MunicipalityId }));

			CreateMap<CreateHousingDto, Housing>()
			.ForMember(dest => dest.Broker, opt => opt.MapFrom(src => new Broker() { BrokerId = src.BrokerId }))
			.ForMember(dest => dest.Category, opt => opt.MapFrom(src => new HousingCategory() { HousingCategoryId = src.CategoryId }))
			.ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => new Municipality() { MunicipalityId = src.MunicipalityId }));

			CreateMap<RegisterBrokerDto, Broker>()
				.ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

			CreateMap<AdminEditBrokerDto, Broker>()
				.ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
				.ForPath(dest => dest.User.FirstName, opt => opt.MapFrom(x => x.FirstName))
				.ForPath(dest => dest.User.LastName, opt => opt.MapFrom(x => x.LastName))
				.ForPath(dest => dest.User.Email, opt => opt.MapFrom(x => x.Email))
				.ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(x => x.PhoneNumber));

			CreateMap<EditBrokerDto, Broker>()
				.ForMember(dest => dest.ProfileImage, opt => opt.Ignore());
        }
	}
}
