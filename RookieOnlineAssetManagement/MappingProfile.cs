using AutoMapper;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.AssetService;
using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;
using RookieOnlineAssetManagement.Entities.Dtos.ReportService;
using RookieOnlineAssetManagement.Entities.Dtos.RequestService;
using RookieOnlineAssetManagement.Entities.Dtos.UserService;
using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RookieOnlineAssetManagement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDetailsDto>()
                .ForMember(dest => dest.FullName, act => act.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Gender, act => act.MapFrom(src => (Gender)src.Gender))
                .ForMember(dest => dest.Location, act => act.MapFrom(src => (Location)src.Location))
                .ReverseMap();

            CreateMap<List<UserDetailsDto>, UsersDto>()
                .ForMember(dest => dest.Users, act => act.MapFrom(src => src));

            CreateMap<Assignment, CreateAssignmentDto>()
                .ReverseMap();
            CreateMap<Assignment, DetailsAssignmentDto>()
                .ForMember(dest => dest.AssetCode, act => act.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, act => act.MapFrom(src => src.Asset.AssetName))
                .ForMember(dest => dest.AssetName, act => act.MapFrom(src => src.Asset.AssetName))
                .ForMember(dest => dest.AssignedTo, act => act.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.AssignedBy, act => act.MapFrom(src => src.Admin.UserName))
                .ForMember(dest => dest.State, act => act.MapFrom(src => (AssignmentState)src.State))
                .ForMember(dest => dest.AssignedBy, act => act.MapFrom(src => src.Admin.UserName))
                .ForMember(dest => dest.Specification, act => act.MapFrom(src => src.Asset.Specification))
                .ForMember(dest => dest.State, act => act.MapFrom(src => (AssignmentState)src.State))
                .ReverseMap();
            CreateMap<List<DetailsAssignmentDto>, AssignmentDto>()
                .ForMember(dest => dest.Assignments, act => act.MapFrom(src => src));
            CreateMap<Assignment, UpdateAssignmentDto>()
                .ReverseMap();
            CreateMap<Asset, DetailsAssetDto>()
                .ForMember(dest => dest.State, act => act.MapFrom(src => (AssetState)src.State))
                .ForMember(dest => dest.Category, act => act.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
            CreateMap<List<DetailsAssetDto>, AssetsDto>()
                .ForMember(dest => dest.Assets, act => act.MapFrom(src => src));

            CreateMap<CreateAssetDto, Asset>()
                .ReverseMap();

            CreateMap<Category, CategoryDto>()
                .ReverseMap();

            CreateMap<Assignment, DetailRequestDto>()
                .ForMember(dest => dest.AssetId, act => act.MapFrom(src => src.Asset.Id))
                .ForMember(dest => dest.AssetCode, act => act.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, act => act.MapFrom(src => src.Asset.AssetName))
                .ForMember(dest => dest.RequestedBy, act => act.MapFrom(src => src.RequestedByUser.UserName))
                .ForMember(dest => dest.AcceptedBy, act => act.MapFrom(src => src.AcceptedByAdmin.UserName))
                .ForMember(dest => dest.AssignedDate, act => act.MapFrom(src => src.AssignedDate))
                .ForMember(dest => dest.ReturnedDate, act => act.MapFrom(src => src.ReturnedDate))
                .ForMember(dest => dest.RequestState, act => act.MapFrom(src => (RequestState)src.RequestState))
                .ReverseMap();
            CreateMap<List<DetailRequestDto>, RequestsDto>()
                .ForMember(dest => dest.Requests, act => act.MapFrom(src => src));
            CreateMap<Asset, DetailReportDto>()
                .ForMember(dest => dest.Category, act => act.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
            CreateMap<List<DetailReportDto>, ReportDto>()
                .ForMember(dest => dest.Reports, act => act.MapFrom(src => src));
        }
    }
}
