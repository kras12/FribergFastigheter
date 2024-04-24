using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.AutoMapper
{
    /// <summary>
	/// An auto mapper profile that contains mappings for converting entity classes to DTO classes.
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: -->
    public class ViewModelToDtoAutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        public ViewModelToDtoAutoMapperProfile()
        {
            CreateMap<BrokerViewModel, BrokerDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt.PreCondition(x => x.ProfileImage != null));
        }
    }
}
