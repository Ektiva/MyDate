using AutoMapper;
using MydateAPI.DTOs;
using MydateAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap<User, UserForListDto>();
            //CreateMap<User, UserForDetailedDto>();
            CreateMap<Photo, PhotosForDetailedDto>();

            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });
            //CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserForRegisterDto, User>();
            //CreateMap<MessageForCreationDto, Message>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<User, UserForContactDto>()
               .ForMember(dest => dest.Name, opt =>
               {
                   opt.MapFrom(src => src.Username);
               })
               .ForMember(dest => dest.Status, opt =>
               {
                   opt.MapFrom(src => src.Status.ToLower());
               });
            CreateMap<Message, ChatToReturnDto>()
                .ForMember(m => m.Who, opt => opt
                    .MapFrom(u => u.SenderId))
                .ForMember(m => m.Message, opt => opt
                    .MapFrom(u => u.Content))
                .ForMember(m => m.Time, opt => opt
                    .MapFrom(u => u.MessageSent))
                .ForMember(m => m.Whose, opt => opt
                    .MapFrom(u => u.RecipientId));
            CreateMap<UserForReturnDto, User>();
        }
    }
}
