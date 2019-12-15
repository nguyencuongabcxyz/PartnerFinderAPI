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

            CreateMap<QuestionPostDetailDto, Post>();
            CreateMap<Post, QuestionPostDetailDto>();

            CreateMap<UserInformation, QuestionPostDetailDto>();
            CreateMap<QuestionPostDetailDto, UserInformation>();

            CreateMap<ResponseCommentDto, UserInformation>();
            CreateMap<UserInformation, ResponseCommentDto>();

            CreateMap<ResponseCommentDto, Comment>();
            CreateMap<Comment, ResponseCommentDto>();

            CreateMap<RequestCommentDto, Comment>();
            CreateMap<Comment, ResponseCommentDto>();

            CreateMap<FeedbackPostDto, Post>();

            CreateMap<Post, FeedbackPostDetailDto>();
            CreateMap<UserInformation, FeedbackPostDetailDto>();

            CreateMap<UserInformation, PartnerFinderDto>();

            CreateMap<ReqPartnerRequestDto, PartnerRequest>();

            CreateMap<UserInformation, ResPartnerRequestDto>()
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SenderAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Name));
            CreateMap<PartnerRequest, ResPartnerRequestDto>();

            CreateMap<UserInformation, PartnershipDto>();
            CreateMap<Partnership, PartnershipDto>();

            CreateMap<UserInformation, NotificationDto>()
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatorAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Name));
            CreateMap<Notification, NotificationDto>();

            CreateMap<ReqMessageDto, Message>();
            CreateMap<Message, ResMessageDto>();
            CreateMap<UserInformation, ResMessageDto>()
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SenderAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Name));

            CreateMap<UserInformation, ConversationDto>()
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatorAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Name));
            CreateMap<Conversation, ConversationDto>();

            CreateMap<UserInformation, ConversationItemDto>()
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatorAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Name));
            CreateMap<Conversation, ConversationItemDto>();
        }
    }
}
