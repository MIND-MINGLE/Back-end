using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Application.Response;
using Application.Request;
using Domain.Entity;
using Application.Request.Account;
using Application.Request.Patient;
using Application.Request.ChatGroupRequest;
using Application.Response.UsersInGroup;
using Application.Request.UsersInGroup;
using Application.Request.ChatMessage;
using Application.Response.ChatMessage;

namespace Application.MyMapper
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //Account
            CreateMap<Account, ResponseAccount>();
            CreateMap<Account, RequestAccount>();
            //Role
            CreateMap<Role, ResponseRole>();

            //Patient
            CreateMap<CreateNewPatientRequest, Patient>()
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));  // Tạo GUID mới

            CreateMap<Patient, ResponsePatient>()
                .ForMember(dest => dest.Dob, opt => opt.Ignore()); // ignore, we custom Date

            // ChatGroup Mapper
            CreateMap<AddChatGroupRequest, ChatGroup>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
               Guid.NewGuid().ToString()
            ));
            // UsersInGroup
            CreateMap<UsersInGroup, GetAllUserInGroupResponse>();
            CreateMap<UsersInGroupRequest, UsersInGroup>()
            .ForMember(dest => dest.UsersInGroupId, opt => opt.MapFrom(src =>
               Guid.NewGuid().ToString()
            ));
            //Chat Message
            CreateMap<ChatMessageRequest, ChatMessage>()
            .ForMember(dest => dest.ChatMessageId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.MessageStatus, opt => opt.MapFrom(src => src.MessageStatus));
            CreateMap<ChatMessage, ChatMessageResponse>();

        }
    }
}
