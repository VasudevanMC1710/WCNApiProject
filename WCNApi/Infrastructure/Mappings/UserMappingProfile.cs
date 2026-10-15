using AutoMapper;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;

namespace WCNApi.Infrastructure.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AddOrUpdateUserRequestDto, LoginUserDetailsModel>()
                .ForMember(dest => dest.Uid,
                    opt => opt.Ignore())
                .ForMember(dest => dest.LoginHistories,
                    opt => opt.Ignore());
        }
    }
}
