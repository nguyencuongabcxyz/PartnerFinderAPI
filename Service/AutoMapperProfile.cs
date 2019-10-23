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

            CreateMap<UserInformation, DashboardPostDto>();
            CreateMap<Post, DashboardPostDto>();
            CreateMap<IEnumerable<UserInformation>, IEnumerable<DashboardPostDto>>();
            CreateMap<IEnumerable<Post>, IEnumerable<DashboardPostDto>>();

            CreateMap<UserInformation, UserInfoDto>();
            CreateMap<UserInfoDto, UserInformation>();

            CreateMap<QuestionPostDto, Post>();
            CreateMap<Post, QuestionPostDto>();
        }
    }
}
