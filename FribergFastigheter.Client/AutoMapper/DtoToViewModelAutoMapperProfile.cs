using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.AutoMapper
{
    public class DtoToViewModelAutoMapperProfile : Profile
    {
        public DtoToViewModelAutoMapperProfile()
        {
            CreateMap<BrokerDto, BrokerViewModel>()
                .ForMember(dest => dest.ProfileImage, opt => opt
                .PreCondition(x => x.ProfileImage != null));

            CreateMap<ImageDto, ImageViewModel>();
        }
    }
}
