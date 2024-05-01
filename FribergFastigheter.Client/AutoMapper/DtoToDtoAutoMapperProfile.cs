using AutoMapper;
using FribergFastigheter.Client.Components;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting DTO classes to other DTO classes.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class DtoToDtoAutoMapperProfile : Profile
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public DtoToDtoAutoMapperProfile()
		{
			CreateMap<HousingDto, DeleteImagesDto>()
				.ForMember(dest => dest.ImageIds, opt => opt.MapFrom(src => src.Images.Select(x => x.ImageId).ToList()));
        }
	}
}
