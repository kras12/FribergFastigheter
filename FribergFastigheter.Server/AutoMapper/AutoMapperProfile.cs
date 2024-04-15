﻿using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheterApi.Data.Entities;

namespace FribergFastigheter.Server.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for entity classes and DTO classes.
	/// </summary>
	/// <!-- Author: Jimmie, Marcus -->
	/// <!-- Co Authors: -->
	public class AutoMapperProfile : Profile
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public AutoMapperProfile()
		{
			CreateMap<Housing, HousingDto>();
			CreateMap<HousingCategory, HousingCategoryDto>();
			CreateMap<Municipality, MunicipalityDto>();

			CreateMap<BrokerFirm, BrokerFirmDto>()
				.ForMember(dest => dest.Logotype, opt => opt.PreCondition(x => x.Logotype != null));

			CreateMap<Broker, BrokerDto>()
				.ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null))
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(scr => $"{scr.FirstName} {scr.LastName}"));

			CreateMap<Image, string>()
				.ConvertUsing(src => src.FileName);
		}		
	}
}