using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheterApi.Data.Entities;

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

			CreateMap<BrokerFirmDto, BrokerFirm>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<BrokerDto, Broker>()
				.ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null));

			CreateMap<UpdateHousingDto, Housing>()
			.ForMember(dest => dest.Broker, opt => opt.MapFrom(src => new Broker() { BrokerId = src.BrokerId }))
			.ForMember(dest => dest.BrokerFirm, opt => opt.MapFrom(src => new BrokerFirm() { BrokerFirmId = src.BrokerFirmId }))
			.ForMember(dest => dest.Category, opt => opt.MapFrom(src => new HousingCategory() { HousingCategoryId = src.CategoryId }))
			.ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => new Municipality() { MunicipalityId = src.MunicipalityId }));

			CreateMap<CreateHousingDto, Housing>()
			.ForMember(dest => dest.Broker, opt => opt.MapFrom(src => new Broker() { BrokerId = src.BrokerId }))
			.ForMember(dest => dest.BrokerFirm, opt => opt.MapFrom(src => new BrokerFirm() { BrokerFirmId = src.BrokerFirmId }))
			.ForMember(dest => dest.Category, opt => opt.MapFrom(src => new HousingCategory() { HousingCategoryId = src.CategoryId }))
			.ForMember(dest => dest.Municipality, opt => opt.MapFrom(src => new Municipality() { MunicipalityId = src.MunicipalityId }));

			CreateMap<CreateBrokerDto, Broker>()
				.ForMember(dest => dest.BrokerFirm, opt => opt.Ignore())
				.ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

			CreateMap<UpdateBrokerDto, Broker>()
				.ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
				.ForMember(dest => dest.BrokerFirm, opt => opt.MapFrom(src => new BrokerFirm() { BrokerFirmId = src.BrokerFirmId }));				
		}
	}
}
