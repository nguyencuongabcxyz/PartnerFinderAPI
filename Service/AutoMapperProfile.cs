using System.Collections.Generic;
using AutoMapper;
using Data.Models;
using Service.Models;

namespace Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UserDto, ApplicationUser>();
            CreateMap<UserInformation, FindingPartnerUserDto>();
            CreateMap<FindingPartnerUser, FindingPartnerUserDto>();
            CreateMap<IEnumerable<UserInformation>, IEnumerable<FindingPartnerUserDto>>();
            CreateMap<IEnumerable<FindingPartnerUser>, IEnumerable<FindingPartnerUserDto>>();
        }
    }
}
